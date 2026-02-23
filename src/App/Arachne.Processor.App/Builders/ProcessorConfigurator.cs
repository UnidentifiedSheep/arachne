using Arachne.Abstractions.Interfaces.HostBuilderConfigurator;
using Arachne.Abstractions.Interfaces.HostBuilders;
using Arachne.Abstractions.Interfaces.Processor;
using Arachne.Abstractions.Models.Options;
using Microsoft.Extensions.DependencyInjection;
using Processor;

namespace Arachne.Processor.App.Builders;

public class ProcessorConfigurator : IProcessorConfigurator, IConfigurator
{
    private Type _dispatcherType = typeof(ResponseDispatcher);
    private Type _tagContainerType = typeof(TagContainer);
    private ResponseDispatcherOptions _dispatcherOptions = new();
    
    public void Build(IAppHostBuilder builder)
    {
        builder.Services.AddSingleton(typeof(IResponseDispatcher), _dispatcherType);
        builder.Services.AddSingleton(typeof(ITagContainer), _tagContainerType);
        builder.Services.AddSingleton(_dispatcherOptions);
    }

    public IProcessorConfigurator WithResponseDispatcher<TDispatcher>() where TDispatcher : IResponseDispatcher
    {
        _dispatcherType = typeof(TDispatcher);
        return this;
    }

    public IProcessorConfigurator WithTagContainer<TContainer>() where TContainer : ITagContainer
    {
        _tagContainerType = typeof(TContainer);
        return this;
    }

    public IProcessorConfigurator WithDispatcherOptions(ResponseDispatcherOptions options)
    {
        _dispatcherOptions = options;
        return this;
    }
}