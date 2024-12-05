namespace Trailblazor.Routing.Configuration;

internal sealed class RoutingConfiguration : IRoutingConfiguration
{
    internal List<INode> InternalFlattenedNodes { get; set; } = [];
    internal List<INode> InternalNodesInHierarchy { get; set; } = [];

    public IReadOnlyList<INode> FlattenedNodes => InternalFlattenedNodes;
    public IReadOnlyList<INode> NodesInHierarchy => InternalNodesInHierarchy;
    public IReadOnlyList<IGroupNode> FlattenedGroupNodes => InternalFlattenedNodes.OfType<IGroupNode>().ToList();
    public IReadOnlyList<IRouteNode> FlattenedRouteNodes => InternalFlattenedNodes.OfType<IRouteNode>().ToList();
}
