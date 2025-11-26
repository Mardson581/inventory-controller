using Inventory.Models;

namespace Inventory.Data.Abstractions.Repository;

public interface IRepository<TClass> where TClass : class
{
    public Task<IEnumerable<TClass>> GetAllAsync();
    public Task<TClass?> GetByIdAsync(int id);
    public Task CreateAsync(TClass model);
    public void Delete(TClass model);
    public Task Delete(int id);
    public void Update(TClass model);
}