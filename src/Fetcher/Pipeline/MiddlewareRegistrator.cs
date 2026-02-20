using Arachne.Abstractions.Interfaces.Fetcher;
using Arachne.Abstractions.Interfaces.Fetcher.Pipeline;

namespace Fetcher.Pipeline;

public class MiddlewareRegistrator : IMiddlewareRegistrator
{
    private readonly List<Type> _middlewares = [];
    private bool _isRegistrationEnded;

    public IEnumerable<Type> RegisteredMiddlewares => _middlewares;

    public void Register<T>() where T : IFetcherMiddleware
    {
        if (_isRegistrationEnded) throw new InvalidOperationException("Middleware registration has already ended.");
        _middlewares.Add(typeof(T));
    }

    public void EndRegistration()
    {
        _isRegistrationEnded = true;
    }
}