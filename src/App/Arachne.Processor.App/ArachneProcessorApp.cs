using Arachne.App.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Processor.App;

public sealed class ArachneProcessorApp : BaseApp
{
    private readonly ServiceProvider _provider;
    public override IServiceProvider Services => _provider;
    public ArachneProcessorApp(IServiceCollection services)
    {
        AddBasicExceptionHandler(services);
        _provider = services.BuildServiceProvider(validateScopes: true);
    }

    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await RunHostedServices(cancellationToken);
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
    
    public override async ValueTask DisposeAsync()
    {
        await _provider.DisposeAsync();
    }
}