using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public class Brand
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(1, ErrorMessage="O nome da marca deve ter pelo menos um caractere")]
    public string? Name { get; set; }
}