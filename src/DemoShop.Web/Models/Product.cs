using System;
using System.ComponentModel.DataAnnotations;

namespace DemoShop.Web.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}