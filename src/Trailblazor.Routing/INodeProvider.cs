using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

/// <summary>
/// Provider for configured <see cref="INode"/>s.
/// </summary>
public interface INodeProvider
{
    /// <summary>
    /// Finds the node with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the target node.</param>
    /// <returns>The <see cref="INode"/> if found.</returns>
    public INode? FindNode(string key);

    /// <summary>
    /// Finds the node with the given <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI of the target node.</param>
    /// <returns>The <see cref="INode"/> if found.</returns>
    public INode? FindNodeByUri(string uri);
}
