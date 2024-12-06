using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Configuration.Validation;

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
        var groupNode = Node.CreateUsingBuilder(key, [], builder: builder);
        return AddNode(groupNode);
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
    public IRoutingConfigurationBuilder AddNodeToNode(string targetGroupNodeKey, INode node)
    {
        var parentGroupNode = FindNode(targetGroupNodeKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupNodeKey}' not found.");
        parentGroupNode.InternalNodes.Add(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetGroupKey, string groupKey, Action<INodeBuilder>? builder = null)
    {
        var parentGroupNode = FindNode(targetGroupKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupKey}' not found.");
        var childGroupNode = Node.CreateUsingBuilder(groupKey, [], parentGroupNode, builder: builder);

        parentGroupNode.InternalNodes.Add(childGroupNode);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToNode<TComponent>(string targetGroupKey, string routeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Action<INodeBuilder>? builder = null) where TComponent : ComponentBase
    {
        return AddNodeToNode(targetGroupKey, routeKey, routeUri, typeof(TComponent), builder);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetGroupKey, string routeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Type componentType, Action<INodeBuilder>? builder = null)
    {
        var parentGroupNode = FindNode(targetGroupKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupKey}' not found.");
        var routeNode = Node.CreateUsingBuilder(routeKey, [routeUri], componentType: componentType, builder: builder);

        parentGroupNode.InternalNodes.Add(routeNode);
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
        var parentGroupNode = FindParentForNode(targetNodeKey);
        RemoveNode(targetNodeKey);

        var newGroupNode = Node.CreateUsingBuilder(targetNodeKey, [], parentGroupNode, builder: builder);
        AddNode(newGroupNode);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode<TComponent>(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Action<INodeBuilder>? builder = null) where TComponent : ComponentBase
    {
        return ReplaceNode(targetNodeKey, routeUri, typeof(TComponent), builder);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Type componentType, Action<INodeBuilder>? builder = null)
    {
        RemoveNode(targetNodeKey);
        var newRouteNode = Node.CreateUsingBuilder(targetNodeKey, [routeUri], componentType: componentType, builder: builder);

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
    public IRoutingConfigurationBuilder MoveNodeToNode(string nodeKey, string targetGroupKey)
    {
        var node = FindNode(nodeKey) ?? throw new RoutingValidationException($"Node with key '{nodeKey}' not found.");
        var targetGroupNode = FindNode(targetGroupKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupKey}' not found.");

        RemoveNode(nodeKey);
        targetGroupNode.InternalNodes.Add(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder RemoveNode(string key)
    {
        var node = FindNode(key) ?? throw new RoutingValidationException($"Node with key '{key}' not found.");
        var parentGroupNode = FindParentForNode(key);

        if (parentGroupNode != null)
            parentGroupNode.InternalNodes.Remove(node);
        else
            _nodes.Remove(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddMetadataToNode(string nodeKey, string key, object value)
    {
        var node = FindNode(nodeKey) ?? throw new RoutingValidationException($"Node with key '{nodeKey}' not found.");
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
        INode? FindNodeInGroup(INode groupNode)
        {
            foreach (var child in groupNode.Nodes)
            {
                if (child.Key == key)
                    return child;

                if (child is INode childGroup)
                {
                    var found = FindNodeInGroup(childGroup);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        foreach (var node in _nodes)
        {
            if (node.Key == key)
                return node;

            if (node is INode groupNode)
            {
                var found = FindNodeInGroup(groupNode);
                if (found != null)
                    return found;
            }
        }

        return null;
    }

    private INode? FindParentForNode(string key)
    {
        INode? FindParentGroupNodeInGroup(INode groupNode)
        {
            if (groupNode.Nodes.Any(node => node.Key == key))
                return groupNode;

            return groupNode.Nodes.OfType<INode>().SingleOrDefault(childGroup => FindParentGroupNodeInGroup(childGroup) != null);
        }

        return _nodes.OfType<INode>().SingleOrDefault(groupNode => FindParentGroupNodeInGroup(groupNode) != null);
    }
}
