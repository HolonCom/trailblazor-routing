using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Configuration.Validation;

namespace Trailblazor.Routing.Configuration;

/// <inheritdoc/>
public sealed class RoutingConfigurationBuilder : IRoutingConfigurationBuilder
{
    private readonly List<INode> _nodes = [];

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode(INode node)
    {
        _nodes.Add(node);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode(string key, Action<IGroupNodeBuilder>? group = null)
    {
        var groupNode = GroupNode.CreateUsingBuilder(key, parentGroupNode: null, builder: group);
        return AddNode(groupNode);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode<TComponent>(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<IRouteNodeBuilder>? route = null)
    {
        return AddNode(key, uri, typeof(TComponent), route);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNode(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<IRouteNodeBuilder>? route = null)
    {
        var routeNode = RouteNode.CreateUsingBuilder(key, uri, componentType, route);
        return AddNode(routeNode);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToGroupNode(string targetGroupNodeKey, INode node)
    {
        var parentGroupNode = FindGroupNode(targetGroupNodeKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupNodeKey}' not found.");
        parentGroupNode.InternalNodes.Add(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToGroupNode(string targetGroupKey, string groupKey, Action<IGroupNodeBuilder>? group = null)
    {
        var parentGroupNode = FindGroupNode(targetGroupKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupKey}' not found.");
        var childGroupNode = GroupNode.CreateUsingBuilder(groupKey, parentGroupNode, group);

        parentGroupNode.InternalNodes.Add(childGroupNode);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToGroupNode<TComponent>(string targetGroupKey, string routeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Action<IRouteNodeBuilder>? route = null) where TComponent : ComponentBase
    {
        return AddNodeToGroupNode(targetGroupKey, routeKey, routeUri, typeof(TComponent), route);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder AddNodeToGroupNode(string targetGroupKey, string routeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Type componentType, Action<IRouteNodeBuilder>? route = null)
    {
        var parentGroupNode = FindGroupNode(targetGroupKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupKey}' not found.");
        var routeNode = RouteNode.CreateUsingBuilder(routeKey, routeUri, componentType, route);

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
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, Action<IGroupNodeBuilder> group)
    {
        var parentGroupNode = FindParentGroupNode(targetNodeKey);
        RemoveNode(targetNodeKey);

        var newGroupNode = GroupNode.CreateUsingBuilder(targetNodeKey, parentGroupNode, group);
        AddNode(newGroupNode);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode<TComponent>(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Action<IRouteNodeBuilder>? route = null) where TComponent : ComponentBase
    {
        return ReplaceNode(targetNodeKey, routeUri, typeof(TComponent), route);
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string routeUri, Type componentType, Action<IRouteNodeBuilder>? route = null)
    {
        RemoveNode(targetNodeKey);
        var newRouteNode = RouteNode.CreateUsingBuilder(targetNodeKey, routeUri, componentType, route);

        AddNode(newRouteNode);
        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceRouteNodeComponent<TComponent>(string key) where TComponent : ComponentBase
    {
        return ReplaceRouteNodeComponent(key, typeof(TComponent));
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder ReplaceRouteNodeComponent(string key, Type componentType)
    {
        var routeNode = FindRouteNode(key) ?? throw new RoutingValidationException($"Route node with key '{key}' not found.");
        routeNode.ComponentType = componentType;

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder MoveNodeToGroupNode(string nodeKey, string targetGroupKey)
    {
        var node = FindNode(nodeKey) ?? throw new RoutingValidationException($"Node with key '{nodeKey}' not found.");
        var targetGroupNode = FindGroupNode(targetGroupKey) ?? throw new RoutingValidationException($"Group node with key '{targetGroupKey}' not found.");

        RemoveNode(nodeKey);
        targetGroupNode.InternalNodes.Add(node);

        return this;
    }

    /// <inheritdoc/>
    public IRoutingConfigurationBuilder RemoveNode(string key)
    {
        var node = FindNode(key) ?? throw new RoutingValidationException($"Node with key '{key}' not found.");
        var parentGroupNode = FindParentGroupNode(key);

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
    public IRoutingConfiguration Build()
    {
        var routingConfiguration = new RoutingConfiguration() { InternalNodesInHierarchy = _nodes, };

        foreach (var node in _nodes)
            FlattenNodesRecursively(node, routingConfiguration.InternalFlattenedNodes);

        return routingConfiguration;
    }

    private void FlattenNodesRecursively(INode node, List<INode> nodes)
    {
        nodes.Add(node);

        if (node is IGroupNode groupNode)
        {
            nodes.AddRange(groupNode.RouteNodes);

            if (groupNode.OwnRouteNode != null)
                nodes.Add(groupNode.OwnRouteNode);

            foreach (var childgroup in groupNode.GroupNodes)
                FlattenNodesRecursively(childgroup, nodes);
        }
    }

    private INode? FindNode(string key)
    {
        INode? FindNodeInGroup(IGroupNode groupNode)
        {
            foreach (var child in groupNode.Nodes)
            {
                if (child.Key == key)
                    return child;

                if (child is IGroupNode childGroup)
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

            if (node is IGroupNode groupNode)
            {
                var found = FindNodeInGroup(groupNode);
                if (found != null)
                    return found;
            }
        }

        return null;
    }

    private IRouteNode? FindRouteNode(string key)
    {
        return FindNode(key) as IRouteNode;
    }

    private IGroupNode? FindGroupNode(string key)
    {
        return FindNode(key) as IGroupNode;
    }

    private IGroupNode? FindParentGroupNode(string key)
    {
        IGroupNode? FindParentGroupNodeInGroup(IGroupNode groupNode)
        {
            if (groupNode.Nodes.Any(node => node.Key == key))
                return groupNode;

            return groupNode.Nodes.OfType<IGroupNode>().SingleOrDefault(childGroup => FindParentGroupNodeInGroup(childGroup) != null);
        }

        return _nodes.OfType<IGroupNode>().SingleOrDefault(groupNode => FindParentGroupNodeInGroup(groupNode) != null);
    }
}
