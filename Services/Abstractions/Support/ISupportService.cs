using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportService
{
    public Task AcceptTonerRequestAsync(int tonerRequestId);
    public Task RejectTonerRequestAsync(int tonerRequestId); // Delete the request
    public Task UpdateTonerRequestStatus(int tonerRequestId, TonerRequestStatus status);
    public Task<List<UserTonerRequest>> GetTonerRequestsAsync();
}