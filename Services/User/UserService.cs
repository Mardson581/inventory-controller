using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Abstractions.User;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Services.User;

public class UserService : IUserService
{
    private readonly InventoryDbContext _context;
    private readonly UserManager<ApplicationUser> _users;

    public UserService(InventoryDbContext context, UserManager<ApplicationUser> users)
    {
        _context = context;
        _users = users;
    }

    public Task<bool> CreateTonerRequest(UserTonerRequest request)
    {
        _context.TonerRequests.Add(request);
        return SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}