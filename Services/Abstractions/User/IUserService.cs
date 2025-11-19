using Inventory.Models;

namespace Inventory.Services.Abstractions.User;

public interface IUserService
{
    public Task<bool> CreateTonerRequest(UserTonerRequest request);
    public Task<bool> CancelTonerRequest(int id);
    public Task<List<UserTonerRequest>> GetAllTonerRequests();
    public Task<List<UserTonerRequest>> GetAllTonerRequestsByStatus(TonerRequestStatus status);
    public Task<UserTonerRequest?> GetByIdAsync(int id);
}