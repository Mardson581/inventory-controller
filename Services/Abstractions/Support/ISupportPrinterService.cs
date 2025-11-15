using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportPrinterService
{
    public Task<bool> CreatePrinterAsync(Printer printer);
    public Task<List<Printer>> GetPrintersAsync();
    public Task<Printer> GetByIdAsync(int printerId);
    public Task<bool> DeletePrinterAsync(int printerId);
}