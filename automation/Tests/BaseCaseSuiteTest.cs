using Highspring.Automation.Core;
using Microsoft.Extensions.Logging;

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

        var workspaceRoot = ResolveWorkspaceRoot();
        var logsDirectory = Path.Combine(workspaceRoot, "automation", "TestResults", "logs");
        Directory.CreateDirectory(logsDirectory);

        var logFilePath = Path.Combine(logsDirectory, $"{caseId}-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss}-{caseExecutionId}.log");

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddProvider(new FileLoggerProvider(logFilePath));
        });

        var logger = loggerFactory.CreateLogger<TCase>();
        var testCase = createCase(logger, runId, caseExecutionId);

        try
        {
            await executeFlow(testCase);
        }
        finally
        {
            testCase.ApplyFinallyCleanup();
        }
    }

    private static string ResolveWorkspaceRoot()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        if (Directory.Exists(Path.Combine(currentDirectory, "automation")))
        {
            return currentDirectory;
        }

        if (string.Equals(Path.GetFileName(currentDirectory), "automation", StringComparison.OrdinalIgnoreCase))
        {
            var parentDirectory = Directory.GetParent(currentDirectory);
            if (parentDirectory is not null)
            {
                return parentDirectory.FullName;
            }
        }

        return currentDirectory;
    }
}
