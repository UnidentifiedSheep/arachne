using Arachne.Abstractions.Abstractions;
using Arachne.Crawler.App;
using Arachne.Processor.App;
using MassTransit;
using Messaging.InMemory.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Dual.App;

public sealed class ArachneDualHostBuilder : AppHostBuilder<ArachneDualApp>
{
    public override IServiceCollection Services { get; }
    public ArachneCrawlerHostBuilder CrawlerHostBuilder { get; }
    public ArachneProcessorHostBuilder ProcessorHostBuilder { get; }
    private bool _built;

    public ArachneDualHostBuilder()
    {
        Services = new ServiceCollection();
        CrawlerHostBuilder = new ArachneCrawlerHostBuilder(Services);
        ProcessorHostBuilder = new ArachneProcessorHostBuilder(Services);
    }
    
    public override ArachneDualApp Build()
    {
        if (_built) throw new InvalidOperationException("HostBuilder can only be built once.");
        
        _built = true;
        CrawlerHostBuilder.Build();
        ProcessorHostBuilder.Build();
        AddMessaging();
        
        return new ArachneDualApp(Services);
    }

    private void AddMessaging()
    {
        Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddInMemoryProcessorConsumers();
            x.AddInMemoryCrawlerConsumers();

            x.UsingInMemory((context, cfg) =>
            {
                context.AddInMemoryProcessorEndPoint(cfg);
                context.AddInMemoryCrawlerEndPoint(cfg);
            });
        });
    }
}