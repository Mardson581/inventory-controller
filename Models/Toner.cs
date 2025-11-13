using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public class Toner
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(1, ErrorMessage="O nome da marca deve ter pelo menos um caractere")]
    public string? Name { get; set; }

    [Required]
    public Color Color { get; set; } = Color.Black;

    [Required]
    [MinLength(1, ErrorMessage="Um toner deve ter pelo menos uma impressora compatível")]
    public List<Printer>? CompatiblePrinters { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa")]
    public int Quantity;

    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa")]
    public int Minimum;
}