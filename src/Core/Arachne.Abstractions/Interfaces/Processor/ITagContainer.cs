namespace Arachne.Abstractions.Interfaces.Processor;

public interface ITagContainer
{
    /// <summary>
    /// Adds tag to processor.
    /// </summary>
    /// <param name="tag">Tag for mapping.</param>
    /// <typeparam name="TProcess">Type of processor.</typeparam>
    void Add<TProcess>(string tag) where TProcess : IResponseProcessor;
    /// <summary>
    /// Adds tags to processor.
    /// </summary>
    /// <param name="tags">Tags for mapping</param>
    /// <typeparam name="TProcess">Type of processor.</typeparam>
    void Add<TProcess>(params string[] tags) where TProcess : IResponseProcessor;
    /// <summary>
    /// Returns processor type for a given tag. Returns null if tag is not found.
    /// </summary>
    /// <param name="tag">Tag for search.</param>
    /// <returns>Type of processor.</returns>
    IReadOnlyList<Type> GetProcessors(string tag);

    /// <summary>
    /// Retrieves the processor types associated with the specified collection of tags.
    /// If no processors are found for a tag, the tag is ignored.
    /// </summary>
    /// <param name="tags">A collection of tags for which to retrieve associated processor types.</param>
    /// <returns>A read-only list of processor types associated with the specified tags.</returns>
    IReadOnlyList<Type> GetProcessors(IEnumerable<string> tags);
}