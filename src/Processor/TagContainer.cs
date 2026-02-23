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

    public void Add<TProcess>(params string[] tags) where TProcess : IResponseProcessor
    {
        foreach (var tag in tags)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(tag);
            Add<TProcess>(tag);
        }
    }

    public IReadOnlyList<Type> GetProcessors(string tag)
    {
        return _mapping.GetValueOrDefault(tag) ?? [];
    }

    public IReadOnlyList<Type> GetProcessors(IEnumerable<string> tags)
    {
        var processors = new HashSet<Type>();
        foreach (var tag in tags) processors.UnionWith(GetProcessors(tag));
        
        return processors.ToList();
    }
}