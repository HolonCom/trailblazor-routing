namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Group of nodes in the routing configuration.
/// </summary>
public interface IGroupNode : INode
{
    /// <summary>
    /// Route optionally representing the group.
    /// </summary>
    public IRouteNode? OwnRoute { get; set; }

    /// <summary>
    /// Parent group of the group.
    /// </summary>
    public IGroupNode? ParentGroup { get; }

    /// <summary>
    /// Child routes of the group.
    /// </summary>
    public IReadOnlyList<IRouteNode> Routes { get; }

    /// <summary>
    /// Child groups of the group.
    /// </summary>
    public IReadOnlyList<IGroupNode> Groups { get; }

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
    /// Method accumulates all child routes, and/or the owned route, whether type of represinting
    /// component is the given <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component representing the target routes.</param>
    /// <returns>The target routes.</returns>
    public List<IRouteNode> FindChildrenAndOrOwn(Type componentType);
}
