namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Routing configuration contains informations about the routing registrations and alike.
/// </summary>
public interface IRoutingConfiguration
{
    /// <summary>
    /// Hierarchical node structure.
    /// </summary>
    public IReadOnlyList<IGroupNode> GroupNodesInHierarchy { get; }

    /// <summary>
    /// All group nodes as a flattened list.
    /// </summary>
    public IReadOnlyList<IGroupNode> FlattenedGroupNodes { get; }

    /// <summary>
    /// All route nodes as a flattened list.
    /// </summary>
    public IReadOnlyList<IRouteNode> FlattenedRouteNodes { get; }

    /// <summary>
    /// All nodes as a flattened list.s
    /// </summary>
    public IReadOnlyList<INode> FlattenedNodes { get; }

    /// <summary>
    /// Method finds the <see cref="INode"/> with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the target node.</param>
    /// <returns>Target <see cref="INode"/> if found.</returns>
    public INode? FindNode(string key);

    /// <summary>
    /// Finds the <see cref="IGroupNode"/> with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the target node.</param>
    /// <returns>Target <see cref="IGroupNode"/> if found.</returns>
    public IGroupNode? FindGroupNode(string key);

    /// <summary>
    /// Finds the <see cref="IRouteNode"/> with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the target node.</param>
    /// <returns>Target <see cref="IRouteNode"/> if found.</returns>
    public IRouteNode? FindRouteNode(string key);

    /// <summary>
    /// Finds the parent <see cref="IGroupNode"/> for the node with the given <paramref name="key"/>.
    /// </summary>
    /// <remarks>
    /// This includes the owned <see cref="IRouteNode"/>s of a <see cref="IGroupNode"/>.
    /// </remarks>
    /// <param name="key">Key of the child node whose parent is to be determined.</param>
    /// <returns>Parent group node of the node with the given <paramref name="key"/> if found.</returns>
    public IGroupNode? FindParentForNode(string key);
}
