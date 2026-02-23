namespace Arachne.Abstractions.Interfaces.Processor;

public interface ITagContainer
{
    void Add<TProcess>(string tag) where TProcess : IResponseProcessor;
    IReadOnlyList<Type> GetProcessors(string tag);
    IReadOnlyList<Type> GetProcessors(IEnumerable<string> tags);
}