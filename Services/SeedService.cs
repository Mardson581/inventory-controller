using Inventory.Models;
using Inventory.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Services;

public class SeedService : ISeedService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<SeedService> _logger;
    private readonly IConfiguration _configuration;

    public SeedService(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();
        _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    public async Task Initialize()
    {
        // Verifica se o usuário padrão existe e cria caso não exista.
        ApplicationUser? admin = await _userManager.FindByNameAsync("admin");
        if (admin != null)
        {
            _logger.LogInformation("O usuário ADMIN já existe.");
            return;
        }

        admin = new ApplicationUser
        {
            UserName = _configuration["DefaultUser:Name"],
            Email = _configuration["DefaultUser:Email"],
        };
        admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, _configuration["DefaultUser:Password"]);

        var result = await _userManager.CreateAsync(admin);
        if (!result.Succeeded)
        {
            _logger.LogCritical("Não foi possível criar o usuário ADMIN, verifique se DefaultUser está no arquivo de configuração!");
            return;
        }
        _logger.LogInformation("O usuário ADMIN foi criado!");

        // Verifica se as roles ADMIN, SUPPORT e USER existem e cria caso não existam.
        string[] roles = { "Admin", "Support", "User" };
        foreach (string role in roles)
        {
            IdentityRole? identityRole = await _roleManager.FindByNameAsync(role);
            if (identityRole != null)
            {
                _logger.LogInformation("A role {ROLE} já existe.", role);
                continue;
            }

            identityRole = new IdentityRole
            {
                Name = role,
                NormalizedName = role.ToUpper()
            };

            result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                _logger.LogCritical("Não foi possível criar a role {ROLE}", role);
            }
            return;
        }
        _logger.LogInformation("Todas as roles foram criadas!");

        // Atribui a role Admin ao usuário
        result = await _userManager.AddToRoleAsync(admin, "Admin");
        if (!result.Succeeded)
        {
            _logger.LogCritical("Não foi possível atribuir a role Admin ao usuário {USER}", admin.UserName);
            return;
        }

        _logger.LogInformation("A role Admin foi atribuída ao usuário {USER}", admin.UserName);
        _logger.LogInformation("Usuário Admin semeado!");
    }
}