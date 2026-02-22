using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Models.Fetcher;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Fetcher.FetcherMiddlewares;

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IFetcherMiddleware
{
    public async Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next)
    {
        if (!logger.IsEnabled(LogLevel.Information))
            return await next(context, token);

        var stopwatch = Stopwatch.StartNew();
        var startTime = DateTime.UtcNow;

        logger.LogInformation("[{id}] Starting HTTP {method} request to {url} at {time}", 
            context.Id, context.Method, context.Url, startTime);

        var result = await next(context, token);
        stopwatch.Stop();

        logger.LogInformation("[{id}] Completed HTTP {method} request to {url} at {time} in {duration}ms. Status: {status}", 
            context.Id, context.Method, context.Url, DateTime.UtcNow, stopwatch.ElapsedMilliseconds, result.StatusCode);
        
        return result;
        
    }
}