namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Group of nodes in the routing configuration.
/// </summary>
public interface IGroupNode : INode
{
    /// <summary>
    /// Internal nodes. This list is required for the framework to be able to add nodes to the group.
    /// </summary>
    internal List<INode> InternalNodes { get; }

    /// <summary>
    /// Route optionally representing the group.
    /// </summary>
    public IRouteNode? OwnRouteNode { get; set; }

    /// <summary>
    /// Parent group of the group.
    /// </summary>
    public IGroupNode? ParentGroupNode { get; internal set; }

    /// <summary>
    /// Total child nodes of the group.
    /// </summary>
    public IReadOnlyList<INode> Nodes { get; }

    /// <summary>
    /// All <see cref="Nodes"/> implementing <see cref="IRouteNode"/>.
    /// </summary>
    public IReadOnlyList<IRouteNode> RouteNodes { get; }

    /// <summary>
    /// All <see cref="Nodes"/> implementing <see cref="IGroupNode"/>.
    /// </summary>
    public IReadOnlyList<IGroupNode> GroupNodes { get; }

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
    public IRouteNode? FindChildOrOwnByUri(string uri);

    /// <summary>
    /// Accumulates all child route nodes, and/or the owned route node, where the representing component
    /// is of the given <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component representing the target routes.</param>
    /// <returns>The target routes.</returns>
    public List<IRouteNode> FindChildrenAndOrOwnByComponentType(Type componentType);

    /// <summary>
    /// Recusively accumulates <see cref="IRouteNode"/>s for the given <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component representing the target routes.</param>
    /// <param name="routeNodes">Accumulated route nodes.</param>
    internal void AccumulateRoutesForType(Type componentType, List<IRouteNode> routeNodes);
}
