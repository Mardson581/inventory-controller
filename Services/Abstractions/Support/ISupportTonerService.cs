using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportTonerService
{
    public Task<Result> CreateTonerAsync(Toner toner);
    public Task<IEnumerable<Toner>> GetTonersAsync();
    public Task<Toner?> GetByIdAsync(int id);
    public Task<Result> DeleteTonerAsync(int tonerId);
}