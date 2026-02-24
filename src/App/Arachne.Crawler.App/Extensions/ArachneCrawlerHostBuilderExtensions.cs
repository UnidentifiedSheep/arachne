using Arachne.Crawler.App.Adapters;
using Arachne.Crawler.App.ApiControllers;
using Arachne.Crawler.App.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Arachne.Crawler.App.Extensions;

public static class ArachneCrawlerHostBuilderExtensions
{
    public static ArachneCrawlerHostBuilder WithApi(this ArachneCrawlerHostBuilder builder, string url = "http://localhost:9696/")
    {
        builder.Services.AddSingleton<SwanLoggerAdapter>();
        builder.Services.AddHostedService<ApiBackgroundService>(sp => 
            new ApiBackgroundService(sp.GetRequiredService<ILogger<ApiBackgroundService>>(), sp, url));
        builder.Services.AddTransient<CrawlerJobController>();

        return builder;
    }
}