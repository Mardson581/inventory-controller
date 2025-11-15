using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportBrandService
{
    public Task<bool> CreateBrandAsync(Brand brand);
    public Task<List<Brand>> GetBrandsAsync();
    public Task<Brand> GetByIdAsync(int brandId);
    public Task<bool> DeleteBrandAsync(int brandId);
}