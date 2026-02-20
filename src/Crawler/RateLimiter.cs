using System.Diagnostics;
using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Models.Options;

namespace Crawler;

public class RateLimiter(CrawlerOptions options) : IRateLimiter
{
    private readonly Lock _lock = new();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    public int MaxRps { get; private set; } = options.MaxRps;
    public int CurrentRps { get; private set; }

    public void ChangeRps(int newRps)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newRps);
        MaxRps = newRps;
    }

    public async Task WaitTillAllowed()
    {
        while (true)
        {
            long elapsedMs;

            lock (_lock)
            {
                elapsedMs = _stopwatch.ElapsedMilliseconds;
                
                if (elapsedMs >= 1000)
                {
                    _stopwatch.Restart();
                    CurrentRps = 0;
                }

                if (CurrentRps < MaxRps)
                {
                    CurrentRps++;
                    return;
                }
            }
            
            var waitTime = 1000 - elapsedMs;
            if (waitTime > 0)
                await Task.Delay((int)waitTime);
        }
    }
}