using Arachne.App.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Dual.App;

public sealed class ArachneDualApp : BaseApp
{
    private readonly ServiceProvider _provider;
    public override IServiceProvider Services => _provider;
    
    public ArachneDualApp(IServiceCollection services)
    {
        AddBasicExceptionHandler(services);
        _provider = services.BuildServiceProvider(validateScopes: true);
    }
    
    public override async Task RunAsync(CancellationToken token = default)
    {
        await RunHostedServices(token);
        await Task.Delay(Timeout.Infinite, token);
    }

    public override async ValueTask DisposeAsync()
    {
        await _provider.DisposeAsync();
    }
}