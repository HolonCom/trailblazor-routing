using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Configuration.Validation;

namespace Trailblazor.Routing.Configuration;

/// <inheritdoc/>
public sealed class RouteNodeBuilder : IRouteNodeBuilder
{
    private readonly RouteNode _routeNode;

    internal RouteNodeBuilder(string key, IEnumerable<string> uris, Type routeComponentType)
    {
        _routeNode = new()
        {
            Key = key,
            InternalUris = uris.Select(u => u.Trim('/').Trim('\\')).ToList(),
            ComponentType = routeComponentType,
        };
    }

    /// <inheritdoc/>
    public IRouteNodeBuilder WithMetadata(string key, object value)
    {
        _routeNode.SetMetadataValue(key, value);
        return this;
    }

    /// <inheritdoc/>
    public IRouteNodeBuilder WithAdditionalUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        uri = uri.Trim('/').Trim('\\');
        RoutingValidationException.ThrowIfUriAlreadyExistsForRouteNode(_routeNode, uri);

        _routeNode.InternalUris.Add(uri);
        return this;
    }

    /// <inheritdoc/>
    public IRouteNode Build()
    {
        return _routeNode;
    }
}
