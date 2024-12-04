namespace Trailblazor.Routing.Configuration;

internal abstract class Node : INode
{
    private readonly Dictionary<string, object?> _metadata = [];

    public required string Key { get; init; }
    public IReadOnlyDictionary<string, object?> Metadata => _metadata;

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
