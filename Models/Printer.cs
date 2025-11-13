using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public class Printer
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public Brand? Brand { get; set; }
}