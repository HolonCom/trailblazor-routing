using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

public sealed class RouteResolveResult
{
    public IRouteNode? RouteNode { get; init; }
    public Dictionary<string, object> ComponentParameters { get; init; } = [];

    public static RouteResolveResult Empty => new();
}
