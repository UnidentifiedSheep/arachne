using Microsoft.Extensions.Logging;
using ISwanLogger = Swan.Logging.ILogger;
using SwanLoggLevel = Swan.Logging.LogLevel;
using SwanLogMessageReceivedEventArgs = Swan.Logging.LogMessageReceivedEventArgs;
namespace Arachne.Crawler.App.Adapters;

public sealed class SwanLoggerAdapter(ILogger<SwanLoggerAdapter> logger) : ISwanLogger
{
    public SwanLoggLevel LogLevel =>
        logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug) ? SwanLoggLevel.Debug :
        logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information) ? SwanLoggLevel.Info :
        logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Warning) ? SwanLoggLevel.Warning :
        logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Error) ? SwanLoggLevel.Error :
        SwanLoggLevel.Info;


    public void Log(SwanLogMessageReceivedEventArgs logEvent)
    {
        switch (logEvent.MessageType)
        {
            case SwanLoggLevel.Debug: Debug(logEvent.Message); break;
            case SwanLoggLevel.Info: Info(logEvent.Message); break;
            case SwanLoggLevel.Warning: Warn(logEvent.Message); break;
            case SwanLoggLevel.Error: Error(logEvent.Exception, logEvent.Message); break;
        }
    }

    private void Debug(string message)
    {
        if (logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug)) logger.LogDebug("{Message}", message);
    }

    private void Info(string message)
    {
        if (logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information)) logger.LogInformation("{Message}", message);
    }

    private void Warn(string message) => logger.LogWarning("{Message}", message);
    private void Error(Exception? ex, string message) => logger.LogError(ex, "{Message}", message);
    public void Dispose() { }
}
