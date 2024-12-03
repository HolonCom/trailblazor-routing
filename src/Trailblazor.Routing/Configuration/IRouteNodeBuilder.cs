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
    /// Completes the configuration process and builds the configured <see cref="IRouteNode"/>.
    /// </summary>
    /// <returns>The configured <see cref="IRouteNode"/>.</returns>
    internal IRouteNode Build();
}
