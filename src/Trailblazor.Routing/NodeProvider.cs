using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

internal sealed class NodeProvider(IRoutingConfigurationProvider _routingConfigurationProvider) : INodeProvider
{
    public INode? FindNode(string key)
    {
        return _routingConfigurationProvider.GetRoutingConfiguration().FlattenedNodes.SingleOrDefault(x => x.Key == key);
    }

    public INode? FindNodeByUri(string uri)
    {
        return _routingConfigurationProvider.GetRoutingConfiguration().FlattenedNodes.SingleOrDefault(x => x.HasUri(uri));
    }
}
