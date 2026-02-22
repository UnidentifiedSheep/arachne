using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Contracts.Events;
using MassTransit;

namespace Crawler.Consumers;

public class AddCrawlJobConsumer(ICrawler crawler) : IConsumer<AddCrawlJobEvent>
{
    public Task Consume(ConsumeContext<AddCrawlJobEvent> context)
    {
        foreach (var job in context.Message.Jobs) crawler.AddCrawlJob(job);
        return Task.CompletedTask;
    }
}