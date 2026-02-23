using Arachne.Abstractions.Interfaces.App;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Abstractions.Interfaces.HostBuilders;

public interface IAppHostBuilder
{
    IServiceCollection Services { get; }
    IApp Build();
}