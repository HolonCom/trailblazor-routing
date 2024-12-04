namespace Trailblazor.Routing.Configuration;

internal sealed class GroupNode : Node, IGroupNode
{
    public List<INode> InternalNodes { get; set; } = [];

    public IRouteNode? OwnRouteNode { get; set; }
    public IGroupNode? ParentGroupNode { get; set; }

    public IReadOnlyList<INode> Nodes => InternalNodes;
    public IReadOnlyList<IRouteNode> RouteNodes => Nodes.OfType<IRouteNode>().ToList();
    public IReadOnlyList<IGroupNode> GroupNodes => Nodes.OfType<IGroupNode>().ToList();

    internal static IGroupNode CreateUsingBuilder(string key, IGroupNode? parentGroupNode = null, Action<IGroupNodeBuilder>? builder = null)
    {
        var groupNodeBuilder = new GroupNodeBuilder(key, parentGroupNode);
        builder?.Invoke(groupNodeBuilder);
        return groupNodeBuilder.Build();
    }

    public INode? FindChildOrItselfByKey(string key)
    {
        // 1. Is the group already the target node?
        if (Key == key)
            return this;

        // 2. Is the own route the target?
        if (OwnRouteNode != null && OwnRouteNode.Key == key)
            return OwnRouteNode;

        // 3. Is any of the direct child routes the target node?
        foreach (var childRoute in InternalNodes)
        {
            if (childRoute.Key == key)
                return childRoute;
        }

        // 4. Recurse over the child groups
        foreach (var group in GroupNodes)
        {
            var targetNode = group.FindChildOrItselfByKey(key);
            if (targetNode != null)
                return targetNode;
        }

        return null;
    }

    public IRouteNode? FindChildOrOwnByUri(string uri)
    {
        // 1. Is the own route the target?
        if (OwnRouteNode != null && OwnRouteNode.Uri == uri)
            return OwnRouteNode;

        // 2. Is any of the direct child routes the target route?
        foreach (var routeNode in RouteNodes)
        {
            if (routeNode.Uri == uri)
                return routeNode;
        }

        // 3. Recurse over the child groups
        foreach (var groupNode in GroupNodes)
        {
            var targetRoute = groupNode.FindChildOrOwnByUri(uri);
            if (targetRoute != null)
                return targetRoute;
        }

        return null;
    }

    public List<IRouteNode> FindChildrenAndOrOwnByComponentType(Type componentType)
    {
        var foundRoutes = new List<IRouteNode>();
        AccumulateRoutesForType(componentType, foundRoutes);

        return foundRoutes;
    }

    public void AccumulateRoutesForType(Type componentType, List<IRouteNode> routeNodes)
    {
        if (OwnRouteNode != null && OwnRouteNode.ComponentType == componentType)
            routeNodes.Add(OwnRouteNode);

        routeNodes.AddRange(RouteNodes.Where(r => r.ComponentType == componentType));

        foreach (var groupNode in GroupNodes)
            groupNode.AccumulateRoutesForType(componentType, routeNodes);
    }
}
