using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

/// <summary>
/// Result of a route node resolve operation.
/// </summary>
public sealed class RouteResolveResult
{
    /// <summary>
    /// Resolved route node.
    /// </summary>
    public INode? RouteNode { get; init; }

    /// <summary>
    /// Parsed parameters of the route node componenet.
    /// </summary>
    public Dictionary<string, object> ComponentParameters { get; init; } = [];

    /// <summary>
    /// Creates an empty result.
    /// </summary>
    public static RouteResolveResult Empty => new();
}
