using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Bson;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Tests.DI;
using Trailblazor.Routing.Tests.Mocks;

namespace Trailblazor.Routing.Tests.Configuration;

public class RoutingConfigurationBuilderTests
{
    private readonly Func<IServiceProvider, IRoutingConfiguration> _getConfigurationAction = (sp) => sp.GetRequiredService<IRoutingConfigurationProvider>().GetRoutingConfiguration();

    [Fact]
    public void AddNode_SpecifiedNonComponentType_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Node", "/node", typeof(RoutingConfigurationBuilderTests));
        });

        Assert.Throws<RoutingValidationException>(() => _getConfigurationAction(serviceProvider));
    }

    [Fact]
    public void AddNodeToNode_AddsNodeSuccessfully()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Parent");
            builder.AddNodeToNode("Parent", "Child");
        });

        var parentNode = _getConfigurationAction(serviceProvider).NodesInHierarchy.SingleOrDefault();

        Assert.NotNull(parentNode);
        Assert.True(parentNode.FindChildOrItselfByKey("Child") != null);
    }

    [Fact]
    public void AddNodeToNode_AddsToNotExistingNode_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNodeToNode("NotExistingNode", "Child");
        });

        Assert.Throws<RoutingValidationException>(() => _getConfigurationAction(serviceProvider));
    }

    [Fact]
    public void AddNodeToNode_SpecifiedNonComponentType_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Parent");
            builder.AddNodeToNode("Parent", "Child", "/child", typeof(RoutingConfigurationBuilderTests));
        });

        Assert.Throws<RoutingValidationException>(() => _getConfigurationAction(serviceProvider));
    }

    [Fact]
    public void ReplaceNode_ReplacesNodeSuccessfully()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Node");
            builder.ReplaceNode("Node", n => n.WithMetadata("replaced", true));
        });

        var node = _getConfigurationAction(serviceProvider).NodesInHierarchy.SingleOrDefault();

        Assert.NotNull(node);
        Assert.True(node.HasMetadataValue("replaced"));
    }

    [Fact]
    public void ReplaceNode_RemovesChildNodesOfTheReplacedNode()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Node", n => n.AddNode("ChildNode"));
            builder.ReplaceNode("Node", n => n.WithMetadata("replaced", true));
        });

        var node = _getConfigurationAction(serviceProvider).NodesInHierarchy.SingleOrDefault();

        Assert.NotNull(node);
        Assert.True(node.FindChildOrItselfByKey("ChildNode") == null);
        Assert.DoesNotContain(_getConfigurationAction(serviceProvider).FlattenedNodes, n => n.Key == "ChildNode");
    }

    [Fact]
    public void ReplaceNodeComponent_ReplacedNodeComponentSuccessfully()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode<ComponentMock>("Node", "/node");
            builder.ReplaceNodeComponent<OtherComponentMock>("Node");
        });
    }

    [Fact]
    public void ReplaceNodeComponent_SpecifiedNonComponentType_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode<ComponentMock>("Node", "/node");
            builder.ReplaceNodeComponent("Node", typeof(RoutingConfigurationBuilderTests));
        });

        Assert.Throws<RoutingValidationException>(() => _getConfigurationAction(serviceProvider));
    }

    [Fact]
    public void MoveNodeToNode_MovingParentToChild_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Parent", n => n.AddNode("Child"));
            builder.MoveNodeToNode("Parent", "Child");
        });

        Assert.Throws<RoutingValidationException>(() => _getConfigurationAction(serviceProvider));
    }

    [Fact]
    public void MoveNodeToNode_MovingChildToAnotherParentSuccessfully()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Parent-A", n => n.AddNode("Child"));
            builder.AddNode("Parent-B");
            builder.MoveNodeToNode("Child", "Parent-B");
        });

        var routingConfiguration = _getConfigurationAction(serviceProvider);

        Assert.Empty(routingConfiguration.FlattenedNodes.Single(p => p.Key == "Parent-A").Nodes);
        Assert.Contains(routingConfiguration.FlattenedNodes.Single(p => p.Key == "Parent-B").Nodes, n => n.Key == "Child");
    }

    [Fact]
    public void RemoveNode_RemovesNodeSuccessFully()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Parent-A");
            builder.AddNode("Parent-B", n => n.AddNode("Child"));
            builder.RemoveNode("Parent-B");
        });

        var routingConfiguration = _getConfigurationAction(serviceProvider);

        Assert.Single(routingConfiguration.FlattenedNodes);
        Assert.DoesNotContain(routingConfiguration.FlattenedNodes, n => n.Key == "Parent-B");
    }

    [Fact]
    public void RemoveNode_RemovesNestedNodeSuccessfully()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Parent-A");
            builder.AddNode("Parent-B", n => n.AddNode("Child", n => n.AddNode("ChildOfChild")));
            builder.RemoveNode("Child");
        });

        var routingConfiguration = _getConfigurationAction(serviceProvider);

        Assert.Equal(2, routingConfiguration.FlattenedNodes.Count);
        Assert.DoesNotContain(routingConfiguration.FlattenedNodes, n => n.Key == "Child");
    }

    [Fact]
    public void UseComponentOnNotFound_SpecifiedNonComponentType_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.UseComponentOnNotFound(typeof(RoutingConfigurationBuilderTests));
        });

        Assert.Throws<RoutingValidationException>(() => _getConfigurationAction(serviceProvider));
    }
}
