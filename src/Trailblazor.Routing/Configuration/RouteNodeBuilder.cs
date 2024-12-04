namespace Trailblazor.Routing.Configuration;

/// <inheritdoc/>
public sealed class RouteNodeBuilder : IRouteNodeBuilder
{
    private readonly RouteNode _route;

    internal RouteNodeBuilder(string key, string uri, Type routeComponentType)
    {
        _route = new()
        {
            Key = key,
            Uri = uri.TrimStart('/').TrimStart('\\'),
            ComponentType = routeComponentType,
        };
    }

    /// <inheritdoc/>
    public IRouteNodeBuilder WithMetadata(string key, object value)
    {
        _route.SetMetadataValue(key, value);
        return this;
    }

    /// <inheritdoc/>
    public IRouteNode Build()
    {
        return _route;
    }
}
