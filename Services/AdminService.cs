using Inventory.Models;
using Inventory.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Services;

public class AdminService : IAdminService
{
    private readonly ILogger<AdminService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(UserManager<ApplicationUser> userManager, ILogger<AdminService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IdentityResult> CreateAdminUserAsync(ApplicationUser user)
    {
        return await CreateUserWithRoleAsync(user, ApplicationUserRoles.Admin);
    }
    
    public async Task<IdentityResult> CreateSupportUserAsync(ApplicationUser user)
    {

        return await CreateUserWithRoleAsync(user, ApplicationUserRoles.Support);
    }
    
    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
    {

        return await CreateUserWithRoleAsync(user, ApplicationUserRoles.User);
    }

    private async Task<IdentityResult> CreateUserWithRoleAsync(ApplicationUser user, string roleName)
    {
        IdentityResult result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogWarning("UserManager.CreateAsync falhou: {ERROR}", string.Join(", ", result.Errors));
            return result;
        }
        
        result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            _logger.LogWarning("UserManager.AddToRoleAsync falhou: {ERROR}", string.Join(", ", result.Errors));
            return result;
        }

        return IdentityResult.Success;
    }
}