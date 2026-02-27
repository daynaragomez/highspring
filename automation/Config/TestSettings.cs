namespace Highspring.Automation.Config;

public sealed class TestSettings
{
    public string BaseUrl { get; init; } = EnvironmentConfig.BaseUrl;
    public string ApiBaseUrl { get; init; } = EnvironmentConfig.ApiBaseUrl;
    public bool Headless { get; init; } = EnvironmentConfig.Headless;

    public static TestSettings Load() => new();
}
