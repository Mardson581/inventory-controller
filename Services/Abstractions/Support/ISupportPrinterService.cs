using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportPrinterService
{
    public Task<Result> CreatePrinterAsync(Printer printer);
    public Task<IEnumerable<Printer>> GetPrintersAsync();
    public Task<Printer> GetByIdAsync(int printerId);
    public Task<Result> DeletePrinterAsync(int printerId);
}