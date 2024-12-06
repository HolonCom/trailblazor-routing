using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Exceptions;

namespace Trailblazor.Routing.Configuration;

/// <inheritdoc/>
public sealed class RoutingConfigurationBuilder : IRoutingConfigurationBuilder
{
    private readonly List<INode> _nodes = [];
    private string? _notFoundRedirectUri;
    private Type? _notFoundComponentType;

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode(INode node)
    {
        _nodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode(string key, Action<INodeBuilder>? builder = null)
    {
        var node = Node.CreateUsingBuilder(key, [], builder: builder);
        return AddNode(node);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode<TComponent>(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null)
    {
        return AddNode(key, uri, typeof(TComponent), builder);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null)
    {
        var routeNode = Node.CreateUsingBuilder(key, [uri], componentType: componentType, builder: builder);
        return AddNode(routeNode);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetNodeKey, INode node)
    {
        var parentNode = FindNode(targetNodeKey) ?? throw new RoutingValidationException($"Node node with key '{targetNodeKey}' not found.");
        parentNode.InternalNodes.Add(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetNodeKey, string key, Action<INodeBuilder>? builder = null)
    {
        var parentNode = FindNode(targetNodeKey) ?? throw new RoutingValidationException($"Node node with key '{targetNodeKey}' not found.");
        var node = Node.CreateUsingBuilder(key, [], parentNode, builder: builder);

        parentNode.InternalNodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToNode<TComponent>(string targetNodeKey, string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null) where TComponent : ComponentBase
    {
        return AddNodeToNode(targetNodeKey, key, uri, typeof(TComponent), builder);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetNodeKey, string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null)
    {
        var parentNode = FindNode(targetNodeKey) ?? throw new RoutingValidationException($"Node node with key '{targetNodeKey}' not found.");
        var node = Node.CreateUsingBuilder(key, [uri], componentType: componentType, builder: builder);

        parentNode.InternalNodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, INode node)
    {
        RemoveNode(targetNodeKey);
        AddNode(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, Action<INodeBuilder> builder)
    {
        var targetNode = FindParentForNode(targetNodeKey);
        RemoveNode(targetNodeKey);

        var newNode = Node.CreateUsingBuilder(targetNodeKey, [], targetNode, builder: builder);
        AddNode(newNode);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode<TComponent>(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null) where TComponent : ComponentBase
    {
        return ReplaceNode(targetNodeKey, uri, typeof(TComponent), builder);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null)
    {
        RemoveNode(targetNodeKey);
        var newRouteNode = Node.CreateUsingBuilder(targetNodeKey, [uri], componentType: componentType, builder: builder);

        AddNode(newRouteNode);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNodeComponent<TComponent>(string key)
        where TComponent : ComponentBase
    {
        return ReplaceNodeComponent(key, typeof(TComponent));
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNodeComponent(string key, Type componentType)
    {
        var routeNode = FindNode(key) ?? throw new RoutingValidationException($"Route node with key '{key}' not found.");
        routeNode.ComponentType = componentType;

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder MoveNodeToNode(string key, string targetNodeKey)
    {
        var node = FindNode(key) ?? throw new RoutingValidationException($"Node with key '{key}' not found.");
        var targetNode = FindNode(targetNodeKey) ?? throw new RoutingValidationException($"Node node with key '{targetNodeKey}' not found.");

        RemoveNode(key);
        targetNode.InternalNodes.Add(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder RemoveNode(string key)
    {
        var node = FindNode(key) ?? throw new RoutingValidationException($"Node with key '{key}' not found.");
        var parentNode = FindParentForNode(key);

        if (parentNode != null)
            parentNode.InternalNodes.Remove(node);
        else
            _nodes.Remove(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddMetadataToNode(string targetNodeKey, string key, object value)
    {
        var node = FindNode(targetNodeKey) ?? throw new RoutingValidationException($"Node with key '{targetNodeKey}' not found.");
        node.SetMetadataValue(key, value);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder RedirectOnNotFound([StringSyntax(StringSyntaxAttribute.Uri)] string uri)
    {
        _notFoundRedirectUri = uri;
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder UseComponentOnNotFound<TComponent>()
        where TComponent : ComponentBase
    {
        return UseComponentOnNotFound(typeof(TComponent));
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder UseComponentOnNotFound(Type componentType)
    {
        RoutingValidationException.ThrowIfTypeIsNotAComponent(componentType);
        _notFoundComponentType = componentType;

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfiguration Build()
    {
        var routingConfiguration = new RoutingConfiguration()
        {
            InternalNodesInHierarchy = _nodes,
            NotFoundRedirectUri = _notFoundRedirectUri,
            NotFoundComponentType = _notFoundComponentType,
        };

        foreach (var node in _nodes)
            FlattenNodesRecursively(node, routingConfiguration.InternalFlattenedNodes);

        return routingConfiguration;
    }

    private void FlattenNodesRecursively(INode node, List<INode> nodes)
    {
        nodes.Add(node);
        foreach (var childNode in node.Nodes)
            FlattenNodesRecursively(childNode, nodes);
    }

    private INode? FindNode(string key)
    {
        INode? FindNodeInNode(INode node)
        {
            foreach (var child in node.Nodes)
            {
                if (child.Key == key)
                    return child;

                var found = FindNodeInNode(node);
                if (found != null)
                    return found;
            }

            return null;
        }

        foreach (var node in _nodes)
        {
            if (node.Key == key)
                return node;

            var found = FindNodeInNode(node);
            if (found != null)
                return found;
        }

        return null;
    }

    private INode? FindParentForNode(string key)
    {
        INode? FindParentNodeInNode(INode node)
        {
            if (node.Nodes.Any(n => n.Key == key))
                return node;

            return node.Nodes.OfType<INode>().SingleOrDefault(n => FindParentNodeInNode(n) != null);
        }

        return _nodes.OfType<INode>().SingleOrDefault(n => FindParentNodeInNode(n) != null);
    }
}
