using Arachne.Abstractions.Interfaces.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace Arachne.Processor.App.Extensions;

public static class ArachneProcessorAppExtensions
{
    /// <summary>
    /// Adds tags to a processor, in registered tag container.
    /// </summary>
    /// <param name="app">The processor app.</param>
    /// <param name="tags">Tags associated with processor.</param>
    /// <typeparam name="TProcessor">Type of processor.</typeparam>
    /// <returns></returns>
    public static ArachneProcessorApp MapTagsToProcessor<TProcessor>(this ArachneProcessorApp app, params string[] tags) 
        where TProcessor : IResponseProcessor
    {
        if (tags.Length == 0) throw new ArgumentException("Tags cannot be empty.", nameof(tags));
        
        var tagContainer = app.Services.GetRequiredService<ITagContainer>();
        tagContainer.Add<TProcessor>(tags);
        return app;
    }
}