using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Route represents a uniquely identifyable component type and URI combination.
/// </summary>
public interface IRouteNode : INode
{
    /// <summary>
    /// Internally mutable collection of <see cref="Uris"/>.
    /// </summary>
    internal List<string> InternalUris { get; }

    /// <summary>
    /// Unique relative URIs of the route.
    /// </summary>
    public IReadOnlyList<string> Uris { get; }

    /// <summary>
    /// Type of component representing the route visually.
    /// </summary>
    public Type ComponentType { get; internal set; }

    /// <summary>
    /// Determines whether any of the <see cref="Uris"/> is the given <paramref name="uri"/>.
    /// </summary>
    /// <param name="uri">URI to be looked for.</param>
    /// <returns><see langword="true"/> if the <paramref name="uri"/> has been found.</returns>
    public bool HasUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri);
}
