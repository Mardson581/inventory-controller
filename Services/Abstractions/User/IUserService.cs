using Inventory.Models;

namespace Inventory.Services.Abstractions.User;

public interface IUserService
{
    public Task<bool> CreateTonerRequest(UserTonerRequest request);
}