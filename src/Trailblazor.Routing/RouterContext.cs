using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

/// <summary>
/// Context of the <see cref="TrailblazorRouter"/>. Contains details about the current route, query parameters and more.
/// </summary>
public sealed record RouterContext
{
    internal RouterContext() { }

    /// <summary>
    /// Current route.
    /// </summary>
    public required IRouteNode? Route { get; init; }

    /// <summary>
    /// Current query parameters.
    /// </summary>
    public IReadOnlyDictionary<string, object> RouteParameters { get; init; } = new Dictionary<string, object>();

    /// <summary>
    /// Current route data.
    /// </summary>
    public required RouteData RouteData { get; init; }
}
