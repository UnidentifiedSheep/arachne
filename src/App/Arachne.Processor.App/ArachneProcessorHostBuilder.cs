using Arachne.Abstractions.Interfaces.HostBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Processor.App;

public class ArachneProcessorHostBuilder : IArachneProcessorHostBuilder
{
    public IServiceCollection Services { get; } = new ServiceCollection();
    private bool _built;
    
    public ArachneProcessorHostBuilder() { }

    public ArachneProcessorApp Build()
    {
        if (_built) throw new InvalidOperationException("HostBuilder can only be built once.");
        _built = true;
        
        return new ArachneProcessorApp(Services);
    }
}