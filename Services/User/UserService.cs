using Inventory.Data.UnitOfWork;
using Inventory.Data.Abstractions.Repository;
using Inventory.DTOs.User;
using Inventory.Models;
using Inventory.Services.Abstractions.User;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Services.User;

public class UserService(UnitOfWork unitOfWork, UserManager<ApplicationUser> users, ILogger<UserService> logger) : IUserService
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly IRepository<UserTonerRequest> _repository = unitOfWork.TonerRequests;
    private readonly UserManager<ApplicationUser> _users = users;
    private readonly ILogger<UserService> _logger;

    public async Task<Result<int>> CreateTonerRequest(CreateUserTonerRequestDTO request)
    {
        var userTonerRequest = new UserTonerRequest
        {
            UserId = request.UserId,
            SupportUserId = request.SupportUserId,
            Status = TonerRequestStatus.Pending,
            TonerRequests = request.TonerRequests.Select(tr => new TonerRequest
            {
                TonerId = tr.TonerId,
                Quantity = tr.Quantity
            }).ToList()
        };

        await _repository.CreateAsync(userTonerRequest);
        return await _unitOfWork.CommitAsync();
    }

    public async Task<Result<int>> CancelTonerRequest(int id)
    {
        UserTonerRequest? request = await _repository.GetByIdAsync(id);
        if (request == null)
        {
            _logger.LogError("UserService.CancelTonerRequest falhou: pedido com id {ID} não existe", id);
            return Result<int>.Failure($"UserService.CancelTonerRequest falhou: pedido com id {id} não existe", 0);
        }
        request.Status = TonerRequestStatus.Canceled;
        return await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<UserTonerRequest>> GetAllTonerRequests()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<UserTonerRequest>> GetAllTonerRequestsByStatus(TonerRequestStatus status)
    {
        IEnumerable<UserTonerRequest> list = await _repository.GetAllAsync();
        return list.Where(tr => tr.Status == status);
    }
}