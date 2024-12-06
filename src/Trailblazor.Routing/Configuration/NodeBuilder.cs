using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Exceptions;

namespace Trailblazor.Routing.Configuration;

/// <inheritdoc/>
public sealed class NodeBuilder : INodeBuilder
{
    private readonly Node _node;

    internal NodeBuilder(string key, IEnumerable<string> uris, INode? parentNode, Type? componentType = null)
    {
        _node = new()
        {
            Key = key,
            ComponentType = componentType,
            ParentNode = parentNode,
            InternalUris = uris.Select(u => u.Trim('/').Trim('\\')).ToList(),
        };
    }

    /// <inheritdoc/>
    public INodeBuilder WithUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        uri = uri.Trim('/').Trim('\\');
        RoutingValidationException.ThrowIfUriAlreadyExistsForRouteNode(_node, uri);

        _node.InternalUris.Add(uri);
        return this;
    }

    /// <inheritdoc/>
    public INodeBuilder AddNode(string key, INode node)
    {
        _node.InternalNodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public INodeBuilder AddNode(string key, Action<INodeBuilder>? builder = null)
    {
        var routeNode = Node.CreateUsingBuilder(key, [], _node, builder: builder);
        return AddNode(key, routeNode);
    }

    /// <inheritdoc/>
    public INodeBuilder AddNode<TComponent>(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null)
        where TComponent : ComponentBase
    {
        return AddNode(key, uri, typeof(TComponent), builder);
    }

    /// <inheritdoc/>
    public INodeBuilder AddNode(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null)
    {
        RoutingValidationException.ThrowIfTypeIsNotAComponent(componentType);

        var routeNode = Node.CreateUsingBuilder(key, [uri], _node, componentType, builder);
        return AddNode(key, routeNode);
    }

    /// <inheritdoc/>
    public INodeBuilder WithMetadata(string key, object value)
    {
        _node.SetMetadataValue(key, value);
        return this;
    }

    /// <summary>
    /// Completes the configuration process and builds the configured <see cref="INode"/>.
    /// </summary>
    /// <returns>The configured <see cref="INode"/>.</returns>
    public INode Build()
    {
        return _node;
    }
}
