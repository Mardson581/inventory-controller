using Microsoft.EntityFrameworkCore;
using Inventory.Services.Abstractions.Support;
using Inventory.Models;
using Inventory.Data;

namespace Inventory.Services.Support;

public class SupportBrandService : ISupportBrandService
{
    private readonly InventoryDbContext _context;

    public SupportBrandService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateBrandAsync(Brand brand)
    {
        await _context.Brands.AddAsync(brand);
        return await SaveChanges();
    }

    public async Task<List<Brand>> GetBrandsAsync()
    {
        return await _context.Brands.ToListAsync();
    }

    public async Task<Brand> GetByIdAsync(int brandId)
    {
        return await _context.Brands.FindAsync(brandId);
    }

    public async Task<bool> DeleteBrandAsync(int brandId)
    {
        Brand? brand = await GetByIdAsync(brandId);
        if (brand == null)
            return false;
        _context.Brands.Remove(brand);
        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}