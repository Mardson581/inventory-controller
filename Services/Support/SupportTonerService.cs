using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Services.Abstractions.Support;
using Inventory.Models;

namespace Inventory.Services.Support;

public class SupportTonerService : ISupportTonerService
{
    private readonly InventoryDbContext _context;
    private readonly ILogger<SupportTonerService> _logger;

    public SupportTonerService(InventoryDbContext context, ILogger<SupportTonerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateTonerAsync(Toner toner)
    {
        await _context.Toners.AddAsync(toner);
        return await SaveChanges();
    }

    public async Task<List<Toner>> GetTonersAsync()
    {
        return await _context.Toners.ToListAsync();
    }

    public async Task<Toner> GetByIdAsync(int id)
    {
        return await _context.Toners.FindAsync(id);
    }

    public async Task<bool> DeleteTonerAsync(int tonerId)
    {
        Toner? toner = await GetByIdAsync(tonerId);
        if (toner == null)
        {
            _logger.LogError("SupportTonerService.DeleteBrandAsync falhou: o Toner com a id {ID} não exite", tonerId);
            return false;
        }
        _context.Toners.Remove(toner);
        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}