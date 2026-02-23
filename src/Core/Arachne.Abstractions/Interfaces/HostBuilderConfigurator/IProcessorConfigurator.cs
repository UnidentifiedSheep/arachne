using Arachne.Abstractions.Interfaces.Processor;
using Arachne.Abstractions.Models.Options;

namespace Arachne.Abstractions.Interfaces.HostBuilderConfigurator;

public interface IProcessorConfigurator
{
    IProcessorConfigurator WithResponseDispatcher<TDispatcher>() where TDispatcher : IResponseDispatcher;
    IProcessorConfigurator WithTagContainer<TContainer>() where TContainer : ITagContainer;
    IProcessorConfigurator WithDispatcherOptions(ResponseDispatcherOptions options);
}