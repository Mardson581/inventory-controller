using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportService
{
    public Task<bool> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId);
    public Task<bool> RejectTonerRequestAsync(string supportUserName, int tonerRequestId); // Delete the request
    public Task<bool> GoDeliverRequestAsync(string supportUserName, int tonerRequestId);
    public Task<List<UserTonerRequest>> GetTonerRequestsAsync();
}