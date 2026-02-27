namespace Highspring.Automation.Api;

public sealed class TestControlClient(string apiBaseUrl)
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(apiBaseUrl.TrimEnd('/'))
    };

    public async Task ResetBaselineAsync(CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsync("/internal/test/v1/reset", content: null, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<OrderSnapshotResponse?> GetOrderSnapshotAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"/internal/test/v1/orders/{orderId}", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        return System.Text.Json.JsonSerializer.Deserialize<OrderSnapshotResponse>(
            payload,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public sealed class OrderSnapshotResponse
    {
        public Guid OrderId { get; init; }
        public string GuestSessionId { get; init; } = string.Empty;
        public decimal Subtotal { get; init; }
        public decimal DiscountTotal { get; init; }
        public decimal TaxTotal { get; init; }
        public decimal GrandTotal { get; init; }
    }
}
