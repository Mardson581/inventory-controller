using Inventory.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Services.Abstractions;

public interface IAdminService
{
    public Task<IdentityResult> CreateAdminUserAsync(ApplicationUser user);
    public Task<IdentityResult> CreateSupportUserAsync(ApplicationUser user);
    public Task<IdentityResult> CreateUserAsync(ApplicationUser user);
}