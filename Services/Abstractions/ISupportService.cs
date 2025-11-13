using Inventory.Models;

namespace Inventory.Services.Abstractions;

public interface ISupportService
{
    public Task CreateTonerAsync(Toner toner);
    public Task<List<Toner>> GetTonersAsync();
    public Task UpdateTonerAsync(Toner toner);
    public Task DeleteTonerAsync(int tonerId);

    public Task CreatePrinterAsync(Printer printer);
    public Task<List<Printer>> GetPrintersAsync();
    public Task UpdatePrinterAsync(Printer printer);
    public Task DeletePrinterAsync(int printerId);

    public Task CreateBrandAsync(Brand brand);
    public Task<List<Brand>> GetBrandsAsync();
    public Task UpdateBrandAsync(Brand brand);
    public Task DeleteBrandAsync(int brandId);

    public Task AcceptTonerRequestAsync(int tonerRequestId);
    public Task RejectTonerRequestAsync(int tonerRequestId); // Delete the request
    public Task UpdateTonerRequestStatus(int tonerRequestId, TonerRequestStatus status);
    public Task<List<UserTonerRequest>> GetTonerRequestsAsync();
}