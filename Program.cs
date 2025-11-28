using Inventory.Data;
using Inventory.Data.UnitOfWork;
using Inventory.Models;
using Inventory.Services.Abstractions.Support;
using Inventory.Services.Support;
using Inventory.Services.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"))
);

builder.Services.AddIdentity<ApplicationUser, IdentityUser>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddSingleton<UnitOfWork>();

builder.Services.AddScoped<ISupportBrandService, SupportBrandService>();
builder.Services.AddScoped<ISupportPrinterService, SupportPrinterService>();
builder.Services.AddScoped<ISupportTonerService, SupportTonerService>();
builder.Services.AddScoped<ISupportService, SupportService>();

var app = builder.Build();

app.UseSession();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Semeia o banco de dados caso o usuário Admin não exista.
// As informações do usuário estão no arquivo de configuração da aplicação
// Verifique o arquivo appsetting.Development.json antes de rodar a aplicação!
await new SeedService(app.Services).Initialize();

app.Run();
