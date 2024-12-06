using Microsoft.AspNetCore.Components;
using System.Reflection;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.Configuration;

internal sealed class InternalScanningProfile(IRoutingOptionsProvider _routingOptionsProvider) : IRoutingProfile
{
    public void ConfigureRoutes(IRoutingConfigurationBuilder builder)
    {
        var nodeComponentTypes = ScanAssembliesForComponentTypes();
        var nodes = new List<(INode Node, string? ParentNodeKey)>();

        foreach (var nodeComponentType in nodeComponentTypes)
            CreateNodeFromType(nodes, nodeComponentType);

        foreach ((INode node, string? parentNodeKey) in nodes.OrderBy(n => n.ParentNodeKey == null))
        {
            if (parentNodeKey == null)
                builder.AddNode(node);
            else
                builder.AddNodeToNode(parentNodeKey, node);
        }
    }

    private void CreateNodeFromType(List<(INode Node, string? ParentNodeKey)> nodes, Type nodeComponentType)
    {
        var nodeUris = nodeComponentType.GetCustomAttributes<RouteAttribute>().Select(a => a.Template).ToList();
        if (nodeUris.Count == 0)
            return;

        var nodeKey = nodeComponentType.GetCustomAttribute<NodeKeyAttribute>()?.NodeKey ?? nodeComponentType.Name;
        var nodeParentKey = nodeComponentType.GetCustomAttribute<NodeParentAttribute>()?.ParentNodeKey;
        var nodeMetadata = nodeComponentType.GetCustomAttributes<NodeMetadataAttribute>().Select(a => new KeyValuePair<string, object>(a.MetadataKey, a.MetadataValue)).ToDictionary();

        var node = Node.CreateUsingBuilder(nodeKey, nodeUris, componentType: nodeComponentType, builder: n =>
        {
            foreach (var metadata in nodeMetadata)
                n.WithMetadata(metadata.Key, metadata.Value);

            n.WithUris(nodeUris.Skip(1).ToList());
        });

        nodes.Add((node, nodeParentKey));
    }

    private List<Type> ScanAssembliesForComponentTypes()
    {
        return _routingOptionsProvider
            .GetRoutingOptions().NodeScanAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(ComponentBase)))
            .Where(c => c.GetCustomAttribute(typeof(RouteAttribute)) != null)
            .ToList();
    }
}
