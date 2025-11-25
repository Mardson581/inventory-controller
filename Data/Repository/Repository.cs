using Inventory.Data.Abstractions.Repository;
using Inventory.Models;

namespace Inventory.Data.Repository;

public class Repository<TClass, TKey> : IRepository<TClass, TKey> where TClass : class
{
    public Task<IEnumerable<TClass>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TClass?> GetByIdAsync(TKey id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateAsync(TClass model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(TClass model)
    {
        throw new NotImplementedException();
    }
}