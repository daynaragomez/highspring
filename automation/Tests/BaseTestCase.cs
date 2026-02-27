using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Xunit;

namespace Highspring.Automation.Tests;

public abstract class BaseTestCase(
    ILogger logger,
    Guid runId,
    Guid caseExecutionId,
    string caseId)
{
    private readonly ILogger _logger = logger;
    private readonly Guid _runId = runId;
    private readonly Guid _caseExecutionId = caseExecutionId;
    private readonly string _caseId = caseId;

    protected void LogInfo(string message)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["RunId"] = _runId,
            ["CaseExecutionId"] = _caseExecutionId,
            ["Case"] = _caseId
        });

        _logger.LogInformation(message);
    }

    protected static void AssertUrlContains(IWebDriver driver, string routeSegment)
    {
        Assert.Contains(routeSegment, driver.Url, StringComparison.OrdinalIgnoreCase);
    }

    protected static void AssertHasValue(string? value)
    {
        Assert.False(string.IsNullOrWhiteSpace(value));
    }

    public virtual void ApplyPostconditions()
    {
        LogInfo("Postconditions: execution evidence is recorded.");
    }

    public virtual void ApplyFinallyCleanup()
    {
        LogInfo("Finally/Cleanup: session cleanup per suite policy.");
    }
}
