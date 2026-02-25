using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;
using Arachne.Abstractions.Models.Options;
using Arachne.Contracts.Events;
using Arachne.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crawler.BackgroundServices;

public class CrawlWorker : BackgroundService
{
    private readonly ILogger<CrawlWorker> _logger;
    private readonly ICrawler _crawler;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRateLimiter _rateLimiter;
    private readonly ICrawlerMetrics _metrics;
    private readonly CrawlerOptions _options;
    private readonly IPipelineExecutor _pipelineExecutor;
    private readonly IServiceScope _scope;

    public CrawlWorker(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<CrawlWorker>>();
        _crawler = _scope.ServiceProvider.GetRequiredService<ICrawler>();
        _publishEndpoint = _scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        _rateLimiter = _scope.ServiceProvider.GetRequiredService<IRateLimiter>();
        _metrics = _scope.ServiceProvider.GetRequiredService<ICrawlerMetrics>();
        _options = _scope.ServiceProvider.GetRequiredService<CrawlerOptions>();
        _pipelineExecutor = _scope.ServiceProvider.GetRequiredService<IPipelineExecutor>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information)) _logger.LogInformation("Crawl worker started.");

        var tasks = Enumerable.Range(0, _options.WorkerCount).Select(_ => WorkerLoop(stoppingToken));
        await Task.WhenAll(tasks);
        
        if (_logger.IsEnabled(LogLevel.Information)) _logger.LogInformation("Crawl worker stopped.");
    }
    
    private async Task WorkerLoop(CancellationToken stoppingToken)
    {
        await foreach (var ctx in _crawler.Reader.ReadAllAsync(stoppingToken))
        {
            await _rateLimiter.WaitTillAllowed().ConfigureAwait(false);
            using var taskTimer = _metrics.MeasureTaskTime();
            
            try
            {
                var result = await _pipelineExecutor.ExecutePipeline(ctx, stoppingToken).ConfigureAwait(false);
                await _publishEndpoint.Publish(new FetchCompletedEvent { Result = result.ToContract() }, stoppingToken)
                    .ConfigureAwait(false);
                _metrics.IncrementSuccess();
            }
            catch (Exception ex)
            {
                await _publishEndpoint.Publish(new FetchFaultedEvent
                {
                    Context = ctx.ToContract(),
                    Exception = ex.Message
                }, stoppingToken).ConfigureAwait(false);
                _metrics.IncrementFailure();
            }
        }
    }

    public override void Dispose()
    {
        _scope.Dispose();
        base.Dispose();
    }
}