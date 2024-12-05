using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Builder for configuring <see cref="IRouteNode"/>s.
/// </summary>
public interface IRouteNodeBuilder
{
    /// <summary>
    /// Adds metadata to the route node.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>The route node builder for further configurations.</returns>
    public IRouteNodeBuilder WithMetadata(string key, object value);

    /// <summary>
    /// Adds an additional URI to the route node.
    /// </summary>
    /// <remarks>
    /// This <paramref name="uri"/> obivously has to be unique.
    /// </remarks>
    /// <param name="uri">Additional URI to the route node.</param>
    /// <returns>The route node builder for further configurations.</returns>
    public IRouteNodeBuilder WithAdditionalUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri);

    /// <summary>
    /// Completes the configuration process and builds the configured <see cref="IRouteNode"/>.
    /// </summary>
    /// <returns>The configured <see cref="IRouteNode"/>.</returns>
    internal IRouteNode Build();
}
