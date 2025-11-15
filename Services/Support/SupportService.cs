using Inventory.Services.Abstractions.Support;
using Inventory.Models;
using Inventory.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services.Support;

public class SupportService : ISupportService
{
    private readonly InventoryDbContext _context;
    private readonly UserManager<ApplicationUser> _users;
    private readonly ILogger<SupportService> _logger;

    public SupportService(InventoryDbContext context, UserManager<ApplicationUser> users, ILogger<SupportService> logger)
    {
        _context = context;
        _users = users;
        _logger = logger;
    }

    public async Task<bool> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        ApplicationUser? support = await _users.FindByNameAsync(supportUserName);
        if (support == null)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: não existe usuário com a id {ID}", supportUserName);
            return false;
        }

        UserTonerRequest? request = await _context.TonerRequests.FindAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return false;
        }

        if (request.Status != TonerRequestStatus.Pending)
        {
            _logger.LogError("SupportService.AcceptTonerRequestAsync falhou: a requisição {ID} não está pendente", tonerRequestId);
            return false;
        }

        request.SupportUser = support;
        request.Status = TonerRequestStatus.Accepted;
        return await SaveChanges();
    }

    public async Task<bool> RejectTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _context.TonerRequests.FindAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.RejectTonerRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return false;
        }

        _context.TonerRequests.Remove(request);
        return await SaveChanges();
    }

    public async Task<bool> GoDeliverRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _context.TonerRequests.FindAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.GoDeliverRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return false;
        }

        if (request.Status != TonerRequestStatus.Accepted && request.SupportUserId == supportUserName)
        {
            _logger.LogError(
                "SupportService.GoDeliverRequestAsync falhou: a requisição com a id {REQID} não está com o status 'aceito' ou não pertence ao usuário {USERID}", 
                tonerRequestId, 
                supportUserName
            );
            return false;
        }
        
        request.Status = TonerRequestStatus.InRoute;
        return await SaveChanges();
    }

    public async Task<bool> CompleteDeliverRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _context.TonerRequests.FindAsync(tonerRequestId);
        if (request == null)
        {
            _logger.LogError("SupportService.GoDeliverRequestAsync falhou: não existe requisição com a id {ID}", tonerRequestId);
            return false;
        }

        if (request.Status != TonerRequestStatus.Accepted && request.SupportUserId == supportUserName)
        {
            _logger.LogError(
                "SupportService.GoDeliverRequestAsync falhou: a requisição com a id {REQID} não está com o status 'aceito' ou não pertence ao usuário {USERID}", 
                tonerRequestId, 
                supportUserName
            );
            return false;
        }
        
        request.Status = TonerRequestStatus.Completed;
        return await SaveChanges();
    }

    public async Task<List<UserTonerRequest>> GetTonerRequestsAsync()
    {
        return await _context.TonerRequests.ToListAsync();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}