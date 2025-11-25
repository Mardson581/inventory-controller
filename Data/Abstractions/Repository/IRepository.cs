using Inventory.Models;

namespace Inventory.Data.Abstractions.Repository;

public interface IRepository<TClass, TKey> where TClass : class
{
    public Task<IEnumerable<TClass>> GetAllAsync();
    public Task<TClass?> GetByIdAsync(TKey id);
    public Task<Result> CreateAsync(TClass model);
    public Task<Result> DeleteAsync(TClass model);
}