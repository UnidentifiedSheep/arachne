using Arachne.Abstractions.Interfaces.App;
using Arachne.Abstractions.Interfaces.HostBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Abstractions.Abstractions;

public abstract class AppHostBuilder<TApp> : IAppHostBuilder where TApp : IApp
{
    public abstract IServiceCollection Services { get; }
    public TApp BuildApp() => (TApp) Build();
    public abstract IApp Build();
}