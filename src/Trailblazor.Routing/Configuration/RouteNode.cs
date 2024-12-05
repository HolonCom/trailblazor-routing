using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

internal sealed class RouteNode : Node, IRouteNode
{
    public List<string> InternalUris { get; init; } = [];

    public IReadOnlyList<string> Uris => InternalUris;
    public required Type ComponentType { get; set; }

    internal static IRouteNode CreateUsingBuilder(string key, IEnumerable<string> uris, Type componentType, Action<IRouteNodeBuilder>? builder = null)
    {
        var routeNodeBuilder = new RouteNodeBuilder(key, uris, componentType);
        builder?.Invoke(routeNodeBuilder);
        return routeNodeBuilder.Build();
    }

    /// <summary>
    /// Determines whether any of the <see cref="Uris"/> is the given <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI to be looked for.</param>
    /// <returns><see langword="true"/> if the <paramref name="uri"/> has been found.</returns>
    public bool HasUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        return Uris.Any(u => u == uri);
    }
}
