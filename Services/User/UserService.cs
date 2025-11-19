using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Abstractions.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services.User;

public class UserService : IUserService
{
    private readonly InventoryDbContext _context;
    private readonly UserManager<ApplicationUser> _users;
    private readonly ILogger<UserService> _logger;

    public UserService(
        InventoryDbContext context,
        UserManager<ApplicationUser> users,
        ILogger<UserService> logger
    )
    {
        _context = context;
        _users = users;
        _logger = logger;
    }

    public Task<bool> CreateTonerRequest(UserTonerRequest request)
    {
        _context.TonerRequests.Add(request);
        return SaveChanges();
    }

    public async Task<bool> CancelTonerRequest(int id)
    {
        UserTonerRequest? request = await GetByIdAsync(id);
        if (request == null)
        {
            _logger.LogError("UserService.CancelTonerRequest falhou: pedido com id {ID} não existe", id);
            return false;
        }
        request.Status = TonerRequestStatus.Canceled;
        return await SaveChanges();
    }

    public async Task<List<UserTonerRequest>> GetAllTonerRequests()
    {
        return await _context.TonerRequests.ToListAsync();
    }

    public async Task<List<UserTonerRequest>> GetAllTonerRequestsByStatus(TonerRequestStatus status)
    {
        return await _context.TonerRequests.Where(r => r.Status == status).ToListAsync();
    }

    public async Task<UserTonerRequest?> GetByIdAsync(int id)
    {
        return await _context.TonerRequests.FindAsync(id);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}