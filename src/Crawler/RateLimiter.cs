using System.Diagnostics;
using Arachne.Abstractions.Interfaces.Crawler;

namespace Crawler;

public class RateLimiter : IRateLimiter
{
    private readonly Lock _lock = new();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    public int MaxRps { get; private set; }
    public int CurrentRps { get; private set; }

    public RateLimiter(int maxRps = int.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxRps);
        MaxRps = maxRps;
    }
    
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