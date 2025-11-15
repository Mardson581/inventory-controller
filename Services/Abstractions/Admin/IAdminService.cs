using Inventory.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Services.Abstractions.Admin;

public interface IAdminService
{
    public Task<IdentityResult> CreateAdminUserAsync(ApplicationUser user);
    public Task<ApplicationUser?> GetUserAsync(string userName);
    public Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    public Task<IdentityResult> DeleteUserAsync(string userName);
    public Task<IdentityResult> CreateSupportUserAsync(ApplicationUser user);
    public Task<IdentityResult> CreateUserAsync(ApplicationUser user);
    public Task<List<ApplicationUser>> GetUsersAsync();
}