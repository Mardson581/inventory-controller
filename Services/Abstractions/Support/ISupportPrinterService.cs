using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportPrinterService
{
    public Task<Result<int>> CreatePrinterAsync(Printer printer);
    public Task<IEnumerable<Printer>> GetPrintersAsync();
    public Task<Printer> GetByIdAsync(int printerId);
    public Task<Result<int>> DeletePrinterAsync(int printerId);
}