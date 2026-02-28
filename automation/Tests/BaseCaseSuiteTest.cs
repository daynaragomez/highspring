using Highspring.Automation.Core;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace Highspring.Automation.Tests;

public abstract class BaseCaseSuiteTest : BaseUiTest
{
    protected async Task RunCase<TCase>(
        string caseId,
        Func<ILogger<TCase>, Guid, Guid, TCase> createCase,
        Func<TCase, Task> executeFlow)
        where TCase : BaseTestCase
    {
        var runId = Guid.NewGuid();
        var caseExecutionId = Guid.NewGuid();

        var projectDirectory = ResolveAutomationProjectDirectory();
        var logsDirectory = Path.Combine(projectDirectory, "TestResults", "logs");
        Directory.CreateDirectory(logsDirectory);

        var logFilePath = Path.Combine(logsDirectory, $"{caseId}-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss}-{caseExecutionId}.log");

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            builder.AddProvider(new FileLoggerProvider(logFilePath));
        });

        var logger = loggerFactory.CreateLogger<TCase>();
        var testCase = createCase(logger, runId, caseExecutionId);

        try
        {
            await executeFlow(testCase);
        }
        catch (Exception ex)
        {
            var screenshotPath = TryCaptureFailureScreenshot(caseId, caseExecutionId);
            if (!string.IsNullOrWhiteSpace(screenshotPath))
            {
                logger.LogWarning(ex, "Case {CaseId} failed. Screenshot saved at {ScreenshotPath}", caseId, screenshotPath);
            }
            else
            {
                logger.LogWarning(ex, "Case {CaseId} failed. Screenshot capture was not available.", caseId);
            }

            throw;
        }
        finally
        {
            testCase.ApplyFinallyCleanup();
        }
    }

    private static string ResolveAutomationProjectDirectory()
    {
        var candidates = new[]
        {
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory
        };

        foreach (var candidate in candidates)
        {
            var directory = new DirectoryInfo(candidate);
            while (directory is not null)
            {
                var csprojPath = Path.Combine(directory.FullName, "Highspring.Automation.csproj");
                if (File.Exists(csprojPath))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }
        }

        throw new InvalidOperationException("Could not locate Highspring.Automation.csproj to resolve deterministic log output path.");
    }

    private string? TryCaptureFailureScreenshot(string caseId, Guid caseExecutionId)
    {
        try
        {
            if (Driver is not ITakesScreenshot screenshotDriver)
            {
                return null;
            }

            var projectDirectory = ResolveAutomationProjectDirectory();
            var screenshotsDirectory = Path.Combine(projectDirectory, "TestResults", "screenshots");
            Directory.CreateDirectory(screenshotsDirectory);

            var screenshotPath = Path.Combine(
                screenshotsDirectory,
                $"{caseId}-FAIL-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss}-{caseExecutionId}.png");

            var screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(screenshotPath);

            return screenshotPath;
        }
        catch
        {
            return null;
        }
    }
}
