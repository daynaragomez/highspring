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
}
