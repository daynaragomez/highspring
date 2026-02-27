using Microsoft.Extensions.Logging;

namespace Highspring.Automation.Core;

public sealed class FileLoggerProvider(string logFilePath) : ILoggerProvider
{
    private readonly object _sync = new();

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(logFilePath, categoryName, _sync);
    }

    public void Dispose()
    {
    }

    private sealed class FileLogger(string filePath, string categoryName, object sync) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            var line = $"{DateTimeOffset.UtcNow:O} [{logLevel}] {categoryName} {message}";
            if (exception is not null)
            {
                line += $" | {exception}";
            }

            lock (sync)
            {
                File.AppendAllText(filePath, line + Environment.NewLine);
            }
        }
    }
}
