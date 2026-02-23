using Arachne.Abstractions.Interfaces;
using Arachne.Abstractions.Interfaces.App;
using Arachne.App.Base.ExceptionHandlers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.App.Base;

public abstract class BaseApp : IApp 
{
    public abstract IServiceProvider Services { get; }
    private bool _exceptionHandlerRegistered;
    
    public abstract Task RunAsync(CancellationToken cancellationToken = default);
    protected void RegisterExceptionHandler(Action<Exception> handler)
    {
        if (_exceptionHandlerRegistered) throw new InvalidOperationException("Exception handler already registered");
        AppDomain.CurrentDomain.UnhandledException += (_, err) =>
        {
            if (err.ExceptionObject is Exception exception) 
                handler(exception);
        };
        TaskScheduler.UnobservedTaskException += (_, err) =>
        {
            handler(err.Exception);
            err.SetObserved();
        };
        _exceptionHandlerRegistered = true;
    }
    
    protected void RegisterExceptionHandler(Func<Exception, Task> handler)
    {
        if (_exceptionHandlerRegistered) throw new InvalidOperationException("Exception handler already registered");
        AppDomain.CurrentDomain.UnhandledException += async (_, err) =>
        {
            if (err.ExceptionObject is Exception exception) 
                await handler(exception);
        };
        TaskScheduler.UnobservedTaskException += async (_, err) =>
        {
            await handler(err.Exception);
            err.SetObserved();
        };
        
        _exceptionHandlerRegistered = true;
    }
    
    protected void AddBasicExceptionHandler(IServiceCollection services)
    {
        services.AddSingleton<BasicExceptionHandler>();
        services.AddSingleton<IExceptionHandler>(sp => sp.GetRequiredService<BasicExceptionHandler>());
    }

    protected async Task RunBus(CancellationToken token = default)
    {
        var bus = Services.GetRequiredService<IBusControl>();

        await bus.StartAsync(token);

        await using var registration = token.Register(() =>
            bus.StopAsync(CancellationToken.None).GetAwaiter().GetResult());
    }

    public virtual void WithExceptionHandler<THandler>() where THandler : IExceptionHandler
    {
        var exceptionHandler = Services.GetRequiredService<THandler>();
        RegisterExceptionHandler(exceptionHandler.HandleAsync);
    }

    public virtual void WithExceptionHandler(Action<Exception> handler)
    {
        RegisterExceptionHandler(handler);
    }

    public virtual void WithExceptionHandler(Func<Exception, Task> handler)
    {
        RegisterExceptionHandler(handler);
    }
    public virtual ValueTask DisposeAsync() => ValueTask.CompletedTask;
}