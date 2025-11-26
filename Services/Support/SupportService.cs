using Inventory.Services.Abstractions.Support;
using Inventory.Models;
using Inventory.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Inventory.Data.UnitOfWork;
using Inventory.Data.Abstractions.Repository;

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

    public async Task<Result> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        ApplicationUser? support = await _users.FindByNameAsync(supportUserName);
        if (support == null)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: não existe usuário com a id {ID}", supportUserName);
            return Result.Failure($"Não existe usuário com a id {supportUserName}");
        }

        UserTonerRequest? request = await _repository.GetByIdAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return Result.Failure($"Não existe requisição com a id {tonerRequestId}");
        }

        if (request.Status != TonerRequestStatus.Pending)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: a requisição {ID} não está pendente", tonerRequestId);
            return Result.Failure($"A requisição {tonerRequestId} não está pendente");
        }

        request.SupportUser = support;
        request.Status = TonerRequestStatus.Accepted;
        return await _unitOfWork.CommitAsync();
    }

    public async Task<Result> RejectTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _repository.GetByIdAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.RejectTonerRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return Result.Failure($"Não existe requisição com a id {tonerRequestId}");
        }

        await _repository.Delete(tonerRequestId);
        return await _unitOfWork.CommitAsync();
    }

    public async Task<Result> GoDeliverRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _repository.GetByIdAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.GoDeliverRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return Result.Failure($"Não existe requisição com a id {tonerRequestId}");
        }

        if (request.Status != TonerRequestStatus.Accepted && request.SupportUserId == supportUserName)
        {
            _logger.LogError(
                "SupportService.GoDeliverRequestAsync falhou: a requisição com a id {REQID} não está com o status 'aceito' ou não pertence ao usuário {USERID}", 
                tonerRequestId, 
                supportUserName
            );
            return Result.Failure($"A requisição com id {tonerRequestId} não está com o status 'aceito' ou não pertence ao usuário");
        }
        
        request.Status = TonerRequestStatus.InRoute;
        return await _unitOfWork.CommitAsync();
    }

    public async Task<Result> CompleteDeliverRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _repository.GetByIdAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.GoDeliverRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return Result.Failure($"Não existe requisição com a id {tonerRequestId}");
        }

        if (request.Status != TonerRequestStatus.Accepted && request.SupportUserId == supportUserName)
        {
            _logger.LogError(
                "SupportService.GoDeliverRequestAsync falhou: a requisição com a id {REQID} não está com o status 'aceito' ou não pertence ao usuário {USERID}", 
                tonerRequestId, 
                supportUserName
            );
            return Result.Failure($"a requisição com a id {tonerRequestId} não está com o status 'aceito' ou não pertence ao usuário {supportUserName}");
        }
        
        request.Status = TonerRequestStatus.Completed;
        return await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<UserTonerRequest>> GetTonerRequestsAsync()
    {
        return await _repository.GetAllAsync();
    }
}