using Arachne.Abstractions.Abstractions;
using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Processor.App.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Processor.App;

public class ArachneProcessorHostBuilder : AppHostBuilder<ArachneProcessorApp>
{
    public override IServiceCollection Services { get; } = new ServiceCollection();
    private bool _built;
    private readonly ProcessorConfigurator _processorConfigurator = new ProcessorConfigurator();


    public ArachneProcessorHostBuilder WithProcessorConfigurator(Action<IProcessorConfigurator> configureProcessor)
    {
        configureProcessor(_processorConfigurator);
        return this;
    }
    
    public override ArachneProcessorApp Build()
    {
        if (_built) throw new InvalidOperationException("HostBuilder can only be built once.");
        _built = true;

        _processorConfigurator.Build(this);
        return new ArachneProcessorApp(Services);
    }
}