using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Node in a routing configuration.
/// </summary>
public interface INode
{
    /// <summary>
    /// Internal nodes. This list is required for the framework to be able to add nodes to the node.
    /// </summary>
    internal List<INode> InternalNodes { get; }

    /// <summary>
    /// Internally mutable collection of <see cref="Uris"/>.
    /// </summary>
    internal List<string> InternalUris { get; }

    /// <summary>
    /// Unique key of the node.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Unique relative URIs of the route.
    /// </summary>
    public IReadOnlyList<string> Uris { get; }

    /// <summary>
    /// Type of component representing the route visually.
    /// </summary>
    public Type? ComponentType { get; internal set; }

    /// <summary>
    /// Parent node of the node.
    /// </summary>
    public INode? ParentNode { get; internal set; }

    /// <summary>
    /// Child nodes of the node.
    /// </summary>
    public IReadOnlyList<INode> Nodes { get; }

    /// <summary>
    /// Metadata of the node. Allows for adding and removing metadata values (such as a 'hidden'-flag for a menu) to and from a node.
    /// </summary>
    public IReadOnlyDictionary<string, object?> Metadata { get; }

    /// <summary>
    /// Determines whether any of the <see cref="Uris"/> is the given <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI to be looked for.</param>
    /// <returns><see langword="true"/> if the <paramref name="uri"/> has been found.</returns>
    public bool HasUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri);

    /// <summary>
    /// Finds the child node, or itself, with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the target node.</param>
    /// <returns>The target node if found.</returns>
    public INode? FindChildOrItselfByKey(string key);

    /// <summary>
    /// Finds the child route, or the owned route, with the given <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI of the target route.</param>
    /// <returns>The target route if found.</returns>
    public INode? FindChildOrItselfByUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri);

    /// <summary>
    /// Accumulates all child route nodes, and/or the owned route node, where the representing component
    /// is of the given <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component representing the target routes.</param>
    /// <returns>The target routes.</returns>
    public List<INode> FindChildrenAndOrItselfByComponentType(Type componentType);

    /// <summary>
    /// Method fetches the nodes metadata value for the specified <paramref name="key"/> and casts it into the <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of metadata value.</typeparam>
    /// <param name="key">Key of the desired value.</param>
    /// <param name="defaultValue">Optional default value in case the desired value has not been found.</param>
    /// <returns>Found metadata value for the specified <paramref name="key"/>.</returns>
    /// <exception cref="InvalidCastException">Thrown if the fetched value is not of type <typeparamref name="TValue"/>.</exception>
    public TValue? GetMetadataValue<TValue>(string key, TValue? defaultValue = default);

    /// <summary>
    /// Method fetches the nodes metadata value for the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the desired value.</param>
    /// <param name="defaultValue">Optional default value in case the desired value has not been found.</param>
    /// <returns>Found metadata value for the specified <paramref name="key"/>.</returns>
    public object? GetMetadataValue(string key, object? defaultValue = default);

    /// <summary>
    /// Method sets the metadata <paramref name="value"/> for the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the <paramref name="value"/> to be set.</param>
    /// <param name="value">Value to be set.</param>
    public void SetMetadataValue(string key, object? value);

    /// <summary>
    /// Method removes the nodes metadata value with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the metadata value that is to be removed.</param>
    public void RemoveMetadataValue(string key);

    /// <summary>
    /// Method determines whether the node has a metadata value with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key that is to be checked for.</param>
    /// <returns><see langword="true"/> if the nodes metadata contains a value with the specified <paramref name="key"/>.</returns>
    public bool HasMetadataValue(string key);

    /// <summary>
    /// Recusively accumulates <see cref="INode"/>s for the given <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component representing the target routes.</param>
    /// <param name="nodes">Accumulated route nodes.</param>
    internal void AccumulateRoutesForType(Type componentType, List<INode> nodes);
}
