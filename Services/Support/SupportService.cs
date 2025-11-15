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

    public SupportService(InventoryDbContext context, UserManager<ApplicationUser> users)
    {
        _context = context;
        _users = users;
    }

    public async Task<bool> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        ApplicationUser? support = await _users.FindByNameAsync(supportUserName);
        if (support == null)
            return false;
        UserTonerRequest? request = await _context.TonerRequests.FindAsync(tonerRequestId);
        if (request == null)
            return false;

        request.SupportUser = support;
        request.Status = TonerRequestStatus.Accepted;
        return await SaveChanges();
    }

    public async Task<bool> RejectTonerRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _context.TonerRequests.FindAsync(tonerRequestId);
        if (request == null)
            return false;
        _context.TonerRequests.Remove(request);
        return await SaveChanges();
    }

    public async Task<bool> GoDeliverRequestAsync(string supportUserName, int tonerRequestId)
    {
        UserTonerRequest? request = await _context.TonerRequests.FindAsync(tonerRequestId);
        if (request == null)
            return false;
        if (request.Status != TonerRequestStatus.Accepted && request.SupportUserId == supportUserName)
            return false;
        request.Status = TonerRequestStatus.InRoute;
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