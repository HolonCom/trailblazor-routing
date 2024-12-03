using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Route represents a uniquely identifyable component type and URI combination.
/// </summary>
public interface IRouteNode : INode
{
    /// <summary>
    /// Unique relative URI of the route.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Uri)]
    public string Uri { get; }

    /// <summary>
    /// Type of component representing the route visually.
    /// </summary>
    public Type ComponentType { get; }
}
