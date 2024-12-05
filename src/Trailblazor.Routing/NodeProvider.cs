using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

internal sealed class NodeProvider(IRoutingConfigurationProvider _routingConfigurationProvider) : INodeProvider
{
    public INode? FindNode(string key)
    {
        return _routingConfigurationProvider.GetRoutingConfiguration().FlattenedGroupNodes.SingleOrDefault(x => x.Key == key);
    }

    public IRouteNode? FindRouteNode(string key)
    {
        return FindNode(key) as IRouteNode;
    }

    public IRouteNode? FindRouteNodeByUri(string uri)
    {
        return _routingConfigurationProvider.GetRoutingConfiguration().FlattenedRouteNodes.SingleOrDefault(x => x.HasUri(uri));
    }

    public IGroupNode? FindGroupNode(string key)
    {
        return FindNode(key) as IGroupNode;
    }
}
