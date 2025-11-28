using Inventory.Data.UnitOfWork;
using Inventory.Data.Abstractions.Repository;
using Inventory.Services.Abstractions.Support;
using Inventory.Models;

namespace Inventory.Services.Support;

public class SupportTonerService : ISupportTonerService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IRepository<Toner> _repository;
    private readonly ILogger<SupportTonerService> _logger;

    public SupportTonerService(UnitOfWork unitOfWork, ILogger<SupportTonerService> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.Toners;
        _logger = logger;
    }

    public async Task<Result<int>> CreateTonerAsync(Toner toner)
    {
        _logger.LogInformation("Criando Toner {NAME}", toner.Name);
        await _repository.CreateAsync(toner);
        return await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Toner>> GetTonersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Toner?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Result<int>> DeleteTonerAsync(int tonerId)
    {
        _logger.LogWarning("Deletando toner com id {ID}", tonerId);
        await _repository.Delete(tonerId);
        return await _unitOfWork.CommitAsync();
    }
}