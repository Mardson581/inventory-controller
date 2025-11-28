using Inventory.Models;

namespace Inventory.Data.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    public Task<Result<int>> CommitAsync();
}