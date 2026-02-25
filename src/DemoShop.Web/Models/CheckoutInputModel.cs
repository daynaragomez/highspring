using System.ComponentModel.DataAnnotations;

namespace DemoShop.Web.Models;

public class CheckoutInputModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }

    [Required]
    [Display(Name = "Address Line 1")]
    public string AddressLine1 { get; set; } = string.Empty;

    [Display(Name = "Address Line 2")]
    public string? AddressLine2 { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Postal Code")]
    [RegularExpression(@"^[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d$", ErrorMessage = "Postal code must be in the format A1A 1A1.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    public string ShippingMethod { get; set; } = "Standard";

    [Required]
    public string PaymentMethod { get; set; } = "Card";
}
