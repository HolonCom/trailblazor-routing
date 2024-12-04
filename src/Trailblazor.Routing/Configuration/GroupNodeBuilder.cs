using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Configuration.Validation;

namespace Trailblazor.Routing.Configuration;

/// <inheritdoc/>
public sealed class GroupNodeBuilder : IGroupNodeBuilder
{
    private readonly GroupNode _groupNode;

    internal GroupNodeBuilder(string key, IGroupNode? parentGroup = null)
    {
        _groupNode = new()
        {
            Key = key,
            ParentGroupNode = parentGroup,
        };
    }

    /// <inheritdoc/>
    public IGroupNodeBuilder AddNode(string key, INode node)
    {
        _groupNode.InternalNodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IGroupNodeBuilder AddNode(string key, Action<IGroupNodeBuilder>? group = null)
    {
        var groupNode = GroupNode.CreateUsingBuilder(key, _groupNode, group);
        return AddNode(key, groupNode);
    }

    /// <inheritdoc/>
    public IGroupNodeBuilder AddNode<TComponent>(string key, string uri, Action<IRouteNodeBuilder>? route = null)
        where TComponent : ComponentBase
    {
        return AddNode(key, uri, typeof(TComponent), route);
    }

    /// <inheritdoc/>
    public IGroupNodeBuilder AddNode(string key, string uri, Type componentType, Action<IRouteNodeBuilder>? route = null)
    {
        RoutingValidationException.ThrowIfTypeIsNotAComponent(componentType);

        var routeNode = RouteNode.CreateUsingBuilder(key, uri, componentType, route);
        return AddNode(key, routeNode);
    }

    /// <inheritdoc/>
    public IGroupNodeBuilder RepresentedByRouteNode<TComponent>(string key, string uri, Action<IRouteNodeBuilder>? route = null)
        where TComponent : ComponentBase
    {
        return RepresentedByRouteNode(key, uri, typeof(TComponent), route);
    }

    /// <inheritdoc/>
    public IGroupNodeBuilder RepresentedByRouteNode(string key, string uri, Type componentType, Action<IRouteNodeBuilder>? route = null)
    {
        RoutingValidationException.ThrowIfTypeIsNotAComponent(componentType);

        var routeNode = RouteNode.CreateUsingBuilder(key, uri, componentType, route);
        _groupNode.OwnRouteNode = routeNode;

        return this;
    }

    /// <inheritdoc/>
    public IGroupNodeBuilder WithMetadata(string key, object value)
    {
        _groupNode.SetMetadataValue(key, value);
        return this;
    }

    /// <inheritdoc/>
    public IGroupNode Build()
    {
        return _groupNode;
    }
}
