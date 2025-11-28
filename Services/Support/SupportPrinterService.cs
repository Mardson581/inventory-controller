using Inventory.Data.UnitOfWork;
using Inventory.Data.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Services.Abstractions.Support;
using Inventory.Models;

namespace Inventory.Services.Support;

public class SupportPrinterService(UnitOfWork unitOfWork, ILogger<SupportPrinterService> logger) : ISupportPrinterService
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly IRepository<Printer> _repository = unitOfWork.Printers;
    private readonly ILogger<SupportPrinterService> _logger = logger;

    public async Task<Result<int>> CreatePrinterAsync(Printer printer)
    {
        _logger.LogInformation("Criando Impressora {NAME}", printer.Name);
        await _repository.CreateAsync(printer);
        return await _unitOfWork.CommitAsync();
    }
    public async Task<IEnumerable<Printer>> GetPrintersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Printer> GetByIdAsync(int printerId)
    {
        return await _repository.GetByIdAsync(printerId);
    }
    
    public async Task<Result<int>> DeletePrinterAsync(int printerId)
    {
        _logger.LogWarning("Deletando impressora com id {ID}", printerId);
        await _repository.Delete(printerId);
        return await _unitOfWork.CommitAsync();
    }
}