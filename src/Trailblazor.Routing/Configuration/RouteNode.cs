using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

internal sealed class RouteNode : Node, IRouteNode
{
    [StringSyntax(StringSyntaxAttribute.Uri)]
    public required string Uri { get; init; }
    public required Type ComponentType { get; set; }

    internal static IRouteNode CreateUsingBuilder(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<IRouteNodeBuilder>? builder = null)
    {
        var routeNodeBuilder = new RouteNodeBuilder(key, uri, componentType);
        builder?.Invoke(routeNodeBuilder);
        return routeNodeBuilder.Build();
    }
}
