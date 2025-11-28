using Inventory.Services.Abstractions.Support;
using Inventory.Models;
using Microsoft.AspNetCore.Identity;
using Inventory.Data.Abstractions.Repository;
using Inventory.Data.UnitOfWork;

namespace Inventory.Services.Support;

public class SupportService : ISupportService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IRepository<UserTonerRequest> _repository;
    private readonly UserManager<ApplicationUser> _users;
    private readonly ILogger<SupportService> _logger;

    public SupportService(UnitOfWork unitOfWork, UserManager<ApplicationUser> users, ILogger<SupportService> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.TonerRequests;
        _users = users;
        _logger = logger;
    }

    public async Task<Result<UserTonerRequest?>> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        ApplicationUser? support = await _users.FindByNameAsync(supportUserName);
        if (support == null)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: não existe usuário com a id {ID}", supportUserName);
            return Result<UserTonerRequest?>.Failure($"Não existe usuário com a id {supportUserName}", null);
        }

        UserTonerRequest? request = await _repository.GetByIdAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return Result<UserTonerRequest?>.Failure($"Não existe requisição com a id {tonerRequestId}", null);
        }

        if (request.Status != TonerRequestStatus.Pending)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: a requisição {ID} não está pendente", tonerRequestId);
            return Result<UserTonerRequest?>.Failure($"A requisição {tonerRequestId} não está pendente", request);
        }

        request.SupportUser = support;
        request.Status = TonerRequestStatus.Accepted;
        
        var commitResult = await _unitOfWork.CommitAsync();
        if (!commitResult.IsSuccess)
        {
            return Result<UserTonerRequest?>.Failure(commitResult.Error, request);
        }

        return Result<UserTonerRequest?>.Success(request);
    }

    public async Task<Result<UserTonerRequest?>> RejectTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _repository.GetByIdAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.RejectTonerRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return Result<UserTonerRequest?>.Failure($"Não existe requisição com a id {tonerRequestId}", null);
        }

        await _repository.Delete(tonerRequestId);
        var commitResult = await _unitOfWork.CommitAsync();
        if (!commitResult.IsSuccess)
        {
            return Result<UserTonerRequest?>.Failure(commitResult.Error, request);
        }
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