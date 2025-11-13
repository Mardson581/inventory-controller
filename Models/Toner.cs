using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public class Toner
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public Color Color { get; set; } = Color.Black;
    [Required]
    public List<Printer>? CompatiblePrinters { get; set; }
    public int Quantity;
    public int Minimum;
}