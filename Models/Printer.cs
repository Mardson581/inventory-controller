using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public class Printer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(1, ErrorMessage="O nome da impressora deve ter pelo menos um caractere")]
    public string? Name { get; set; }

    [Required]
    public Brand? Brand { get; set; }
}