namespace Arachne.Abstractions.Interfaces.HostBuilderConfigurator;

public interface ICrawlerOptionsConfigurator
{
    ICrawlerOptionsConfigurator WithMaxRps(int maxRps);
    ICrawlerOptionsConfigurator WithMaxConcurrency(int maxConcurrency);
}