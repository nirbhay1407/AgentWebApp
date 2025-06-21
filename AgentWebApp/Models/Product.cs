using System.ComponentModel.DataAnnotations;

namespace AgentWebApp.Models;

public class Product
{
    [Key]
    public int product_id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, 999999.99)]
    public decimal Price { get; set; }

    public bool Status { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}