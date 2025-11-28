using Inventory.Services.Abstractions.Support;
using Inventory.Models;
using Microsoft.AspNetCore.Identity;
using Inventory.Data.Abstractions.Repository;
using Inventory.Data.UnitOfWork;

namespace Inventory.Services.Support;

public class SupportService(UnitOfWork unitOfWork, UserManager<ApplicationUser> users, ILogger<SupportService> logger) : ISupportService
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly IRepository<UserTonerRequest> _repository = unitOfWork.TonerRequests;
    private readonly UserManager<ApplicationUser> _users = users;
    private readonly ILogger<SupportService> _logger = logger;

    public async Task<Result<UserTonerRequest?>> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        var validationResult = await ValidateRequestByIdAsync(tonerRequestId, supportUserName, TonerRequestStatus.Pending);
        
        if (!validationResult.IsSuccess)
            return validationResult;

        UserTonerRequest request = validationResult.Data;
        ApplicationUser support = await _users.FindByNameAsync(supportUserName);
        request.SupportUser = support;
        request.Status = TonerRequestStatus.Accepted;
        
        var commitResult = await _unitOfWork.CommitAsync();
        if (!commitResult.IsSuccess)
            return Result<UserTonerRequest?>.Failure(commitResult.Error, request);

        return Result<UserTonerRequest?>.Success(request);
    }

    public async Task<Result<UserTonerRequest?>> RejectTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        var validationResult = await ValidateRequestByIdAsync(tonerRequestId, supportUserName, TonerRequestStatus.Pending);
        if (!validationResult.IsSuccess)
            return validationResult;

        UserTonerRequest request = validationResult.Data;
        await _repository.Delete(tonerRequestId);
        var commitResult = await _unitOfWork.CommitAsync();
        if (!commitResult.IsSuccess)
            return Result<UserTonerRequest?>.Failure(commitResult.Error, request);
        return Result<UserTonerRequest?>.Success(request);
    }

    public async Task<Result<UserTonerRequest?>> GoDeliverRequestAsync(string supportUserName, int tonerRequestId)
    {
        var validationResult = await ValidateRequestByIdAsync(tonerRequestId, supportUserName, TonerRequestStatus.Accepted);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        
        var request = validationResult.Data;
        request.Status = TonerRequestStatus.InRoute; // Em Rota
        var commitResult = await _unitOfWork.CommitAsync();
        if (!commitResult.IsSuccess)
        {
            return Result<UserTonerRequest?>.Failure(commitResult.Error, request);
        }
        return Result<UserTonerRequest?>.Success(request);
    }

    public async Task<Result<UserTonerRequest?>> CompleteDeliverRequestAsync(string supportUserName, int tonerRequestId)
    {
        var validationResult = await ValidateRequestByIdAsync(tonerRequestId, supportUserName, TonerRequestStatus.InRoute);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var request = validationResult.Data;
        request.Status = TonerRequestStatus.Completed; // Concluído
        var commitResult = await _unitOfWork.CommitAsync();
        if (!commitResult.IsSuccess)
        {
            return Result<UserTonerRequest?>.Failure(commitResult.Error, request);
        }
        return Result<UserTonerRequest?>.Success(request);
    }

    public async Task<IEnumerable<UserTonerRequest>> GetTonerRequestsAsync()
    {
        return await _repository.GetAllAsync();
    }

    private async Task<Result<UserTonerRequest?>> ValidateRequestByIdAsync(int tonerRequestId, string supportUserName, TonerRequestStatus expectedStatus)
    {
        UserTonerRequest? request = await _repository.GetByIdAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("ValidateRequestByIdAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return Result<UserTonerRequest?>.Failure($"Não existe requisição com a id {tonerRequestId}", null);
        }

        if (request.SupportUserId != supportUserName || request.Status != expectedStatus)
        {
            _logger.LogError(
                "ValidateRequestByIdAsync falhou: a requisição {REQID} não pertence ao usuário {USERID} ou não está com o status {STATUS}", 
                tonerRequestId, 
                supportUserName,
                expectedStatus
            );
            return Result<UserTonerRequest?>.Failure($"A requisição com id {tonerRequestId} não pode ser processada.", request);
        }

        return Result<UserTonerRequest?>.Success(request);
    }
}