using System.Threading.Channels;
using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Models.Fetcher;

namespace Crawler;

public sealed class Crawler : ICrawler
{
    private readonly Channel<FetcherContext> _channel = Channel.CreateUnbounded<FetcherContext>();
    public ChannelReader<FetcherContext> Reader => _channel.Reader;
    
    public async ValueTask<(bool, Guid)> AddCrawlJob(FetcherContext context)
    {
        await _channel.Writer.WriteAsync(context);
        return (true, context.Id);
    }

    public ValueTask DisposeAsync()
    {
        _channel.Writer.Complete();
        return ValueTask.CompletedTask;
    }
}