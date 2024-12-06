using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

/// <summary>
/// Provider for configured <see cref="INode"/>s.
/// </summary>
public interface INodeProvider
{
    public INode? FindNode(string key);
    public INode? FindNodeByUri(string uri);
}
