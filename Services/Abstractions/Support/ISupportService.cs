using Inventory.Models;

namespace Inventory.Services.Abstractions.Support;

public interface ISupportService
{
    public Task<Result> AcceptTonerRequestAsync(string supportUserName, int tonerRequestId);
    public Task<Result> RejectTonerRequestAsync(string supportUserName, int tonerRequestId); // Delete the request
    public Task<Result> GoDeliverRequestAsync(string supportUserName, int tonerRequestId);
    public Task<Result> CompleteDeliverRequestAsync(string supportUserName, int tonerRequestId);
    public Task<IEnumerable<UserTonerRequest>> GetTonerRequestsAsync();
}