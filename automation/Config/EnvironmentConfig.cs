namespace Highspring.Automation.Config;

public static class EnvironmentConfig
{
    public static string BaseUrl => Environment.GetEnvironmentVariable("HIGHSPRING_BASE_URL") ?? "http://localhost:8080";
    public static string ApiBaseUrl => Environment.GetEnvironmentVariable("HIGHSPRING_API_BASE_URL") ?? "http://localhost:8081";

    public static bool Headless =>
        (Environment.GetEnvironmentVariable("HIGHSPRING_HEADLESS") ?? "true")
        .Equals("true", StringComparison.OrdinalIgnoreCase);
}
