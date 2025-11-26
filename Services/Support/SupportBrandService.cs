using Inventory.Data.UnitOfWork;
using Inventory.Data.Abstractions.Repository;
using Inventory.Services.Abstractions.Support;
using Inventory.Models;

namespace Inventory.Services.Support;

public class SupportBrandService : ISupportBrandService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IRepository<Brand> _repository;
    private readonly ILogger<SupportBrandService> _logger;

    public SupportBrandService(UnitOfWork unitOfWork, ILogger<SupportBrandService> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.Brands;
        _logger = logger;
    }

    public async Task<Result> CreateBrandAsync(Brand brand)
    {
        await _repository.CreateAsync(brand);
        _logger.LogInformation("Criando Marca {NAME}", brand.Name);
        return await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Brand>> GetBrandsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Brand?> GetByIdAsync(int brandId)
    {
        return await _repository.GetByIdAsync(brandId);
    }

    public async Task<Result> DeleteBrandAsync(int brandId)
    {
        _logger.LogWarning("Deletando marca com id {ID}", brandId);
        await _repository.Delete(brandId);
        return await _unitOfWork.CommitAsync();
    }
}