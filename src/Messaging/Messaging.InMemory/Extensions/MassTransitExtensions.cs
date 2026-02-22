using Crawler.Consumers;
using MassTransit;
using Processor.Consumers;

namespace Messaging.InMemory.Extensions;

public static class MassTransitExtensions
{
    extension(IBusRegistrationContext ctx)
    {
        /// <summary>
        /// Configures an in memory endpoint for processing messages related to FetchCompletedConsumer.
        /// </summary>
        /// <param name="cfg">
        /// The in memory bus factory configurator used to define the receiver endpoint.
        /// </param>
        public void AddInMemoryProcessorEndPoint(IInMemoryBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("processor-queue", ep =>
            {
                ep.ConfigureConsumer<FetchCompletedConsumer>(ctx);
            });
        }

        /// <summary>
        /// Configures an in memory endpoint for processing messages related to AddCrawlJobConsumer.
        /// </summary>
        /// <param name="cfg">
        /// The in memory bus factory configurator used to define the receiver endpoint.
        /// </param>
        public void AddInMemoryCrawlerEndPoint(IInMemoryBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("crawler-queue", ep =>
            {
                ep.ConfigureConsumer<AddCrawlJobConsumer>(ctx);
            });
        }
    }

    extension(IBusRegistrationConfigurator cfg)
    {
        public void AddInMemoryProcessorConsumers()
        {
            cfg.AddConsumer<FetchCompletedConsumer>();
        }
        
        public void AddInMemoryCrawlerConsumers()
        {
            cfg.AddConsumer<AddCrawlJobConsumer>();
        }
    }
}