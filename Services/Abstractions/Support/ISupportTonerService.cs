using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportTonerService
{
    public Task<bool> CreateTonerAsync(Toner toner);
    public Task<List<Toner>> GetTonersAsync();
    public Task<Toner> GetByIdAsync(int id);
    public Task<bool> DeleteTonerAsync(int tonerId);
}