using Arachne.Abstractions.Interfaces.Crawler;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Builder;

public sealed class ArachneApp : IAsyncDisposable
{
    private readonly ServiceProvider _provider;

    public ArachneApp(IServiceCollection services)
    {
        _provider = services.BuildServiceProvider(validateScopes: true);
    }

    public async Task RunAsync(CancellationToken token = default)
    {
        var crawler = _provider.GetRequiredService<ICrawler>();
        await crawler.StartAsync(token);
        await Task.Delay(Timeout.Infinite, token);
    }

    public async ValueTask DisposeAsync()
    {
        await _provider.DisposeAsync();
    }

    public IServiceProvider Services => _provider;
}