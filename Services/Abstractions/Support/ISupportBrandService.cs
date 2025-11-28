using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportBrandService
{
    public Task<Result<int>> CreateBrandAsync(Brand brand);
    public Task<IEnumerable<Brand>> GetBrandsAsync();
    public Task<Brand> GetByIdAsync(int brandId);
    public Task<Result<int>> DeleteBrandAsync(int brandId);
}