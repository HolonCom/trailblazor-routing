using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

internal class Node : INode
{
    private readonly Dictionary<string, object?> _metadata = [];

    public List<string> InternalUris { get; init; } = [];
    public List<INode> InternalNodes { get; set; } = [];

    public required string Key { get; init; }
    public IReadOnlyList<string> Uris => InternalUris;
    public Type? ComponentType { get; set; }
    public INode? ParentNode { get; set; }
    public IReadOnlyList<INode> Nodes => InternalNodes;
    public IReadOnlyDictionary<string, object?> Metadata => _metadata;

    internal static INode CreateUsingBuilder(string key, IEnumerable<string> uris, INode? parentNode = null, Type? componentType = null, Action<INodeBuilder>? builder = null)
    {
        var routeNodeBuilder = new NodeBuilder(key, uris, parentNode, componentType);
        builder?.Invoke(routeNodeBuilder);
        return routeNodeBuilder.Build();
    }

    public bool HasUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        uri = uri.Trim('/').Trim('\\');
        return Uris.Any(u => u == uri);
    }

    public INode? FindChildOrItselfByKey(string key)
    {
        if (Key == key)
            return this;

        foreach (var childRoute in InternalNodes)
        {
            if (childRoute.Key == key)
                return childRoute;
        }

        foreach (var node in Nodes)
        {
            var targetNode = node.FindChildOrItselfByKey(key);
            if (targetNode != null)
                return targetNode;
        }

        return null;
    }

    public INode? FindChildOrItselfByUri(string uri)
    {
        if (HasUri(uri))
            return this;

        foreach (var node in Nodes)
        {
            var targetNode = node.FindChildOrItselfByUri(uri);
            if (targetNode != null)
                return targetNode;
        }

        return null;
    }

    public List<INode> FindChildrenAndOrItselfByComponentType(Type componentType)
    {
        var foundRoutes = new List<INode>();
        AccumulateRoutesForType(componentType, foundRoutes);

        return foundRoutes;
    }

    public void AccumulateRoutesForType(Type componentType, List<INode> nodes)
    {
        if (ComponentType == componentType)
            nodes.Add(this);

        foreach (var node in Nodes)
            node.AccumulateRoutesForType(componentType, nodes);
    }

    public TValue? GetMetadataValue<TValue>(string key, TValue? defaultValue = default)
    {
        if (_metadata.TryGetValue(key, out var value))
        {
            if (value is not TValue metadataValue)
                throw new InvalidCastException($"Value '{value}' is not of type '{typeof(TValue)}'.");

            return metadataValue;
        }

        return defaultValue;
    }

    public object? GetMetadataValue(string key, object? defaultValue = default)
    {
        return _metadata.TryGetValue(key, out var value) ? value : defaultValue;
    }

    public void SetMetadataValue(string key, object? value)
    {
        _metadata[key] = value;
    }

    public void RemoveMetadataValue(string key)
    {
        _metadata.Remove(key);
    }

    public bool HasMetadataValue(string key)
    {
        return _metadata.ContainsKey(key);
    }
}
