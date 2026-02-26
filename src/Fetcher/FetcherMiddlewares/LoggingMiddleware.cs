using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Models.Fetcher;
using Microsoft.Extensions.Logging;
using Arachne.Abstractions.Interfaces.Crawler;

namespace Fetcher.FetcherMiddlewares;

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger, ICrawlerMetrics metrics) 
    : IFetcherMiddleware
{
    private static long _requestCount;
    public async Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next)
    {
        var result = await next(context, token);
        if (!logger.IsEnabled(LogLevel.Information) || Interlocked.Increment(ref _requestCount) % 5000 != 0)
            return result;

        logger.LogInformation(
            """
            Processed {count} requests. Last request: {method} {url} Status: {status}.
            QueueLength: {queue}, Success: {success}, Failure: {failure},
            AvgRunTime: {avg}ms, MaxRunTime: {max}ms, MinRunTime: {min}ms,
            Current Rps: {rps}
            """,
            _requestCount, context.Method, context.Url, result.StatusCode,
            metrics.QueueLength, metrics.SuccessCount, metrics.FailureCount,
            metrics.AverageRunTimeMs, metrics.MaxRunTimeMs, metrics.MinRunTimeMs,
            metrics.Rps
        );
        
        return result;
    }
}