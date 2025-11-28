using Inventory.Data.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Repository;

public class InventoryRepository<TClass> : IRepository<TClass> where TClass : class
{
    private readonly InventoryDbContext _context;
    private readonly DbSet<TClass> _set;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
        _set = context.Set<TClass>();
    }

    public async Task<IEnumerable<TClass>> GetAllAsync()
    {
        return await _set.AsNoTracking().ToListAsync();
    }

    public async Task<TClass?> GetByIdAsync(int id)
    {
        return await _set.FindAsync(id);
    }

    public async Task CreateAsync(TClass model)
    {
        await _set.AddAsync(model);
    }

    public void Delete(TClass model)
    {
        _set.Remove(model);
    }

    public async Task Delete(int id)
    {
        Delete(await GetByIdAsync(id));
    }

    public void Update(TClass model)
    {
        _set.Update(model);
    }
}