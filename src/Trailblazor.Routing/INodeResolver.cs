namespace Trailblazor.Routing;

/// <summary>
/// Service resolves nodes for a given URI.
/// </summary>
public interface INodeResolver
{
    /// <summary>
    /// Resolves a route node and parses its component parameters from the given <paramref name="relativeUri"/>.
    /// </summary>
    /// <param name="relativeUri">Relative URI to be used for resolving and parsing.</param>
    /// <returns>Result containing information about the resolved route node and parsed component parameters.</returns>
    public NodeResolveResult ResolveNodeForUri(string relativeUri);
}
