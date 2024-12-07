using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

/// <summary>
/// Result of a node resolve operation.
/// </summary>
public sealed class NodeResolveResult
{
    /// <summary>
    /// Resolved node.
    /// </summary>
    public INode? Node { get; init; }

    /// <summary>
    /// Parsed parameters of the route node componenet.
    /// </summary>
    public Dictionary<string, object> ComponentParameters { get; init; } = [];

    /// <summary>
    /// Creates an empty result.
    /// </summary>
    public static NodeResolveResult Empty => new();
}
