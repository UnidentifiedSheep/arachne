using System.Net;
using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Models.Fetcher;

namespace Fetcher.FetcherMiddlewares;

public class RetryMiddleware : IFetcherMiddleware
{
    public async Task<FetcherResult> InvokeAsync(FetcherContext context, CancellationToken token, FetcherDelegate next)
    {
        int attempts = context.RetryCount + 1;
        int delayMs = context.DelayMs;
        HttpStatusCode lastStatus = default;
        
        while (attempts-- > 0)
        {
            var result = await next(context, token);
            lastStatus = result.StatusCode;

            if (!context.RetryOn.Contains(result.StatusCode))
                return result;

            delayMs = await Delay(delayMs, context.DelayMultiplier, token);
        }

        return new FetcherResult(null, lastStatus);
    }
    
    /// <summary>
    /// Delays for the currentDelay and returns new delay. Also adds jitter.
    /// </summary>
    private async Task<int> Delay(int currentDelay, double multiplier, CancellationToken cancellationToken)
    {
        if (currentDelay == 0) return 0;
        await Task.Delay(currentDelay, cancellationToken);
        return (int)(currentDelay * multiplier) + Random.Shared.Next(0, currentDelay / 5);
    }
}