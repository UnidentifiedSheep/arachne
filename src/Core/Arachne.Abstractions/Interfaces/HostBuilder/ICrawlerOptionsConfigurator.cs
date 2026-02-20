namespace Arachne.Abstractions.Interfaces.HostBuilder;

public interface ICrawlerOptionsConfigurator
{
    ICrawlerOptionsConfigurator WithMaxRps(int maxRps);
    ICrawlerOptionsConfigurator WithMaxConcurrency(int maxConcurrency);
}