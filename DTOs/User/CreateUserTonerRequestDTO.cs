namespace Inventory.DTOs.User;

public class CreateUserTonerRequestDTO
{
    public string UserId { get; set; }
    public string SupportUserId { get; set; }
    public List<TonerRequestDTO> TonerRequests { get; } = [];
}