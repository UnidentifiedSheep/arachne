using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.App.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Crawler.App;

public sealed class ArachneCrawlerApp : BaseApp
{
    private readonly ServiceProvider _provider;
    public override IServiceProvider Services => _provider;
    
    public ArachneCrawlerApp(IServiceCollection services)
    {
        AddBasicExceptionHandler(services);
        _provider = services.BuildServiceProvider(validateScopes: true);
    }
    
    public override async Task RunAsync(CancellationToken token = default)
    {
        var crawler = _provider.GetRequiredService<ICrawler>();
        
        await crawler.StartAsync(token);
        await Task.Delay(Timeout.Infinite, token);
    }

    public override async ValueTask DisposeAsync()
    {
        await _provider.DisposeAsync();
    }
}