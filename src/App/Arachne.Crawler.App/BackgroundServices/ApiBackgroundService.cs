using Arachne.Abstractions.Interfaces.Crawler;
using Arachne.Crawler.App.Adapters;
using Arachne.Crawler.App.ApiControllers;
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swan.Logging;

namespace Arachne.Crawler.App.BackgroundServices;
public class ApiBackgroundService : BackgroundService
{
    private readonly WebServer _server;

    private readonly ILogger<ApiBackgroundService> _logger;

    public ApiBackgroundService(ILogger<ApiBackgroundService> logger, IServiceProvider sp, string url)
    {
        Logger.NoLogging();
        Logger.RegisterLogger(sp.GetRequiredService<SwanLoggerAdapter>());
        
        _logger = logger;
        _server = new WebServer(o => o
                .WithUrlPrefix(url)
                .WithMode(HttpListenerMode.EmbedIO))
            .WithLocalSessionManager()
            .WithWebApi("/api", m => m.WithController(() =>
            {
                var crawler = sp.GetRequiredService<ICrawler>();
                return new CrawlerJobController(crawler);
            }).WithController(() =>
            {
                var crawlerMetrics = sp.GetRequiredService<ICrawlerMetrics>();
                var rateLimiter = sp.GetRequiredService<IRateLimiter>();
                return new CrawlerMetricsController(crawlerMetrics, rateLimiter);
            }))
            .WithModule(new ActionModule("/", HttpVerbs.Any, 
                ctx => ctx.SendDataAsync(new { Message = "Error" })));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting web server");
        await _server.RunAsync(stoppingToken);
        _logger.LogInformation("Web server stopped");
    }
}