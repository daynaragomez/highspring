namespace Highspring.Application.UseCases;

public class CheckoutAddress
{
    public required string FullName { get; set; }

    public required string Email { get; set; }

    public required string Phone { get; set; }

    public required string AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public required string City { get; set; }

    public required string StateOrRegion { get; set; }

    public required string PostalCode { get; set; }

    public required string Country { get; set; }
}
