namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Routing configuration contains informations about the routing registrations and alike.
/// </summary>
public interface IRoutingConfiguration
{
    /// <summary>
    /// All nodes as a flattened list.
    /// </summary>
    public IReadOnlyList<INode> FlattenedNodes { get; }

    /// <summary>
    /// Only top-level nodes for a hierarchical data structure.
    /// </summary>
    public IReadOnlyList<INode> NodesInHierarchy { get; }

    /// <summary>
    /// All <see cref="FlattenedNodes"/> of type <see cref="IGroupNode"/>.
    /// </summary>
    public IReadOnlyList<IGroupNode> FlattenedGroupNodes { get; }

    /// <summary>
    /// All <see cref="FlattenedNodes"/> of type <see cref="IRouteNode"/>.
    /// </summary>
    public IReadOnlyList<IRouteNode> FlattenedRouteNodes { get; }
}
