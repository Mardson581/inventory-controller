using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Services.Abstractions.Support;
using Inventory.Models;

namespace Inventory.Services.Support;

public class SupportPrinterService : ISupportPrinterService
{
    private readonly InventoryDbContext _context;
    private readonly ILogger<SupportPrinterService> _logger;

    public SupportPrinterService(InventoryDbContext context, ILogger<SupportPrinterService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreatePrinterAsync(Printer printer)
    {
        await _context.Printers.AddAsync(printer);
        return await SaveChanges();
    }
    public async Task<List<Printer>> GetPrintersAsync()
    {
        return await _context.Printers.ToListAsync();
    }

    public async Task<Printer> GetByIdAsync(int printerId)
    {
        return await _context.Printers.FindAsync(printerId);
    }
    
    public async Task<bool> DeletePrinterAsync(int printerId)
    {
        Printer? printer = await GetByIdAsync(printerId);
        if (printer == null)
        {
            _logger.LogError("SupportPrinterService.DeleteBrandAsync falhou: a Printer com a id {ID} não exite", printerId);
            return false;
        }
        _context.Printers.Remove(printer);
        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}