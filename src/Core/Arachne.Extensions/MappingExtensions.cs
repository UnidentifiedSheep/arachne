using System.Net;
using Arachne.Abstractions.Models.Fetcher;
using FetcherContext = Arachne.Contracts.Models.FetcherContext;
using FetcherResult = Arachne.Contracts.Models.FetcherResult;
using ModelFetcherContext = Arachne.Abstractions.Models.Fetcher.FetcherContext;
using ModelFetcherResult = Arachne.Abstractions.Models.Fetcher.FetcherResult;

namespace Arachne.Extensions;

public static class MappingExtensions
{
    public static FetcherResult ToContract(this ModelFetcherResult model)
    {
        return new FetcherResult
        {
            Context = model.Context.ToContract(),
            Result = model.Result,
            StatusCode = (int)model.StatusCode
        };
    }

    public static FetcherContext ToContract(this ModelFetcherContext model)
    {
        return new FetcherContext
        {
            Id = model.Id,
            Method = model.Method.Method,
            Url = model.Url,
            QueryParameters = model.QueryParameters.ToDictionary(),
            Headers = model.Headers.ToDictionary(),
            DelayMs = model.DelayMs,
            DelayMultiplier = model.DelayMultiplier,
            RetryCount = model.RetryCount,
            RetryOn = model.RetryOn.Select(x => (int)x).ToArray(),
            ProcessorTags = model.ProcessorTags.ToHashSet()
        };
    }


    public static ModelFetcherContext ToModel(this FetcherContext value)
    {
        HttpMethod method = HttpMethod.Parse(value.Method);
        HttpStatusCode[] retryOn = value.RetryOn.Select(x => (HttpStatusCode)x).ToArray();
        var model = new ModelFetcherContext(value.Id, value.Url, method, value.RetryCount, value.DelayMs, value.DelayMultiplier, retryOn);
        foreach (var (key, v) in value.Headers) model.WithHeader(key, v);
        foreach (var (key, v) in value.QueryParameters) model.WithQuery(key, v);
        model.WithProcessorTags(value.ProcessorTags);
        return model;
    }

    public static ModelFetcherResult ToModel(this FetcherResult value)
    {
        return new ModelFetcherResult(value.Result, (HttpStatusCode)value.StatusCode, value.Context.ToModel());
    }

    public static ReadonlyFetcherResult ToReadonlyModel(this FetcherResult value)
    {
        return new ReadonlyFetcherResult(value.Result, (HttpStatusCode)value.StatusCode, value.Context.ToReadonlyModel());
    }

    public static ReadonlyFetcherContext ToReadonlyModel(this FetcherContext value)
    {
        var method = HttpMethod.Parse(value.Method);
        var retryOn = value.RetryOn.Select(x => (HttpStatusCode)x);
        return new ReadonlyFetcherContext(value.Id, value.Url, method, value.RetryCount, value.DelayMs, value.DelayMultiplier,
            retryOn, value.Headers.ToDictionary(), value.QueryParameters.ToDictionary(), value.ProcessorTags);
    }
    
}