using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public class TonerRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Toner? Toner { get; set; }
    public int TonerId { get; set; }

    [Range(1, 50, ErrorMessage = "A quantidade deve estar entre 1 e 50")]
    public int Quantity { get; set; }

    [Required]
    public UserTonerRequest? UserTonerRequest { get; set; }
    public int UserTonerRequestId { get; set; }

}