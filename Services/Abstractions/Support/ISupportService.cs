using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportService
{
    public Task<Result<UserTonerRequest?>> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId);
    public Task<Result<UserTonerRequest?>> RejectTonerRequestAsync(string supportUserName, int tonerRequestId); // Delete the request
    public Task<Result<UserTonerRequest?>> GoDeliverRequestAsync(string supportUserName, int tonerRequestId);
    public Task<Result<UserTonerRequest?>> CompleteDeliverRequestAsync(string supportUserName, int tonerRequestId);
    public Task<IEnumerable<UserTonerRequest>> GetTonerRequestsAsync();
}