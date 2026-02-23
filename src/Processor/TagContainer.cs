using System.Collections.Concurrent;
using Arachne.Abstractions.Interfaces.Processor;

namespace Processor;

public class TagContainer : ITagContainer
{
    private readonly ConcurrentDictionary<string, IReadOnlyList<Type>> _mapping = new();
    
    public void Add<TProcess>(string tag) where TProcess : IResponseProcessor
    {
        _mapping.AddOrUpdate(tag, [typeof(TProcess)], (_, arr) =>
        {
            var ls = arr.ToList();
            ls.Add(typeof(TProcess));
            return ls;
        });
    }

    public IReadOnlyList<Type> GetProcessors(string tag)
    {
        return _mapping.TryGetValue(tag, out var processors) ? processors : [];
    }

    public IReadOnlyList<Type> GetProcessors(IEnumerable<string> tags)
    {
        var processors = new List<Type>();
        foreach (var tag in tags) processors.AddRange(GetProcessors(tag));
        return processors;
    }
}