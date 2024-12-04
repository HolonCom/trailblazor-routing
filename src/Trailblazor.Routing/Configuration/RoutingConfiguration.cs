namespace Trailblazor.Routing.Configuration;

internal sealed class RoutingConfiguration : IRoutingConfiguration
{
    internal List<INode> InternalFlattenedNodes { get; set; } = [];
    internal List<INode> InternalNodesInHierarchy { get; set; } = [];

    public IReadOnlyList<INode> FlattenedNodes => InternalFlattenedNodes;
    public IReadOnlyList<INode> NodesInHierarchy => InternalNodesInHierarchy;
    public IReadOnlyList<IGroupNode> FlattenedGroupNodes => InternalFlattenedNodes.OfType<IGroupNode>().ToList();
    public IReadOnlyList<IRouteNode> FlattenedRouteNodes => InternalFlattenedNodes.OfType<IRouteNode>().ToList();

    public INode? FindNode(string key)
    {
        return FlattenedNodes.SingleOrDefault(g => g.Key == key);
    }

    public IGroupNode? FindGroupNode(string key)
    {
        return FlattenedGroupNodes.SingleOrDefault(g => g.Key == key);
    }

    public IRouteNode? FindRouteNode(string key)
    {
        return FlattenedRouteNodes.SingleOrDefault(r => r.Key == key);
    }

    public IGroupNode? FindParentForNode(string key)
    {
        return FlattenedGroupNodes.SingleOrDefault(g =>
        {
            var ownRepresentingRoute = g.OwnRouteNode != null && g.OwnRouteNode.Key == key;
            var anyOfTheChildNodes = g.Nodes.Any(r => r.Key == key);

            return ownRepresentingRoute || anyOfTheChildNodes;
        });
    }
}
