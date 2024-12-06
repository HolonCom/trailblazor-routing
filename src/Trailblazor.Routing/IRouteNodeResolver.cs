namespace Trailblazor.Routing;

/// <summary>
/// Service resolves route nodes.
/// </summary>
public interface IRouteNodeResolver
{
    /// <summary>
    /// Resolves a route node and parses its component parameters from the given <paramref name="relativeUri"/>.
    /// </summary>
    /// <param name="relativeUri">Relative URI to be used for resolving and parsing.</param>
    /// <returns>Result containing information about the resolved route node and parsed component parameters.</returns>
    public RouteResolveResult ResolveRouteForUri(string relativeUri);
}
