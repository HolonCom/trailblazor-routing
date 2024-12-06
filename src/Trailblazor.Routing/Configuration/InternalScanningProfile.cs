using Microsoft.AspNetCore.Components;
using System.Reflection;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.Configuration;

// TODO -> Construct all nodes, create configured relationships and then add them to the IRoutingConfigurationBuilder

internal sealed class InternalScanningProfile(IRoutingOptionsProvider _routingOptionsProvider) : IRoutingProfile
{
    public void ConfigureRoutes(IRoutingConfigurationBuilder builder)
    {
        var nodeComponentTypes = ScanAssembliesForComponentTypes();
        foreach (var nodeComponentType in nodeComponentTypes)
            ConfigureNodeFromType(builder, nodeComponentType);
    }

    private void ConfigureNodeFromType(IRoutingConfigurationBuilder builder, Type nodeComponentType)
    {
        var nodeUris = nodeComponentType.GetCustomAttributes<RouteAttribute>().Select(a => a.Template).ToList();
        if (nodeUris.Count == 0)
            return;

        var nodeKey = nodeComponentType.GetCustomAttribute<NodeKeyAttribute>()?.NodeKey ?? nodeComponentType.Name;
        var nodeParentKey = nodeComponentType.GetCustomAttribute<NodeParentAttribute>()?.ParentNodeKey;
        var nodeMetadata = nodeComponentType.GetCustomAttributes<NodeMetadataAttribute>().Select(a => new KeyValuePair<string, object>(a.MetadataKey, a.MetadataValue)).ToDictionary();

        if (nodeParentKey != null)
        {
            builder.AddNodeToNode(nodeParentKey, nodeKey, nodeUris.First(), nodeComponentType, n =>
            {
                AddAdditionalUrisToNodeBuilder(n, nodeUris);
                AddMetadataToNodeBuilder(n, nodeMetadata);
            });
        }
        else
        {
            builder.AddNode(nodeKey, nodeUris.First(), nodeComponentType, n =>
            {
                AddAdditionalUrisToNodeBuilder(n, nodeUris);
                AddMetadataToNodeBuilder(n, nodeMetadata);
            });
        }
    }

    private void AddMetadataToNodeBuilder(INodeBuilder builder, Dictionary<string, object> nodeMetadata)
    {
        foreach (var metadata in nodeMetadata)
            builder.WithMetadata(metadata.Key, metadata.Value);
    }

    private void AddAdditionalUrisToNodeBuilder(INodeBuilder builder, List<string> routeUris)
    {
        var additionalRouteUris = routeUris.Skip(1).ToList();
        foreach (var routeUri in additionalRouteUris)
            builder.WithUri(routeUri);
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
