using Inventory.DTOs.User;
using Inventory.Models;

namespace Inventory.Services.Abstractions.User;

public interface IUserService
{
    public Task<Result<int>> CreateTonerRequest(CreateUserTonerRequestDTO request);
    public Task<Result<int>> CancelTonerRequest(int id);
    public Task<IEnumerable<UserTonerRequest>> GetAllTonerRequests();
    public Task<IEnumerable<UserTonerRequest>> GetAllTonerRequestsByStatus(TonerRequestStatus status);
}