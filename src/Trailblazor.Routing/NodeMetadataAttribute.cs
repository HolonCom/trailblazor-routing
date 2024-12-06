namespace Trailblazor.Routing;

/// <summary>
/// Attribute adds a metadat key-value-pair to the components node.
/// </summary>
/// <remarks>
/// Sets the <paramref name="metadataKey"/> and <paramref name="metadataValue"/>.
/// </remarks>
/// <param name="metadataKey">Key of the metadata.</param>
/// <param name="metadataValue">Value of the metadata.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class NodeMetadataAttribute(string metadataKey, object metadataValue) : Attribute
{
    /// <summary>
    /// Key of the metadata.
    /// </summary>
    public string MetadataKey { get; } = metadataKey;

    /// <summary>
    /// Value of the metadata.
    /// </summary>
    public object MetadataValue { get; } = metadataValue;
}
