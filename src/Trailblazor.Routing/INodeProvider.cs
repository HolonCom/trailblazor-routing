using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

/// <summary>
/// Provider for configured <see cref="INode"/>s.
/// </summary>
public interface INodeProvider
{
    public INode? FindNode(string key);
    public IRouteNode? FindRouteNode(string key);
    public IRouteNode? FindRouteNodeByUri(string uri);
    public IGroupNode? FindGroupNode(string key);
}
