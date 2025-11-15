using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public class UserTonerRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public ApplicationUser? User { get; set; }
    public string? UserId { get; set; }

    public ApplicationUser? SupportUser { get; set; }
    public string? SupportUserId;

    [Required]
    public List<TonerRequest>? TonerRequests { get; set; }

    public TonerRequestStatus Status { get; set; } = TonerRequestStatus.Pending;
}