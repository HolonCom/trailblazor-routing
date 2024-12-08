using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Tests.DI;
using Trailblazor.Routing.Tests.Mocks;

namespace Trailblazor.Routing.Tests.Configuration;

public class NodeTests
{
    private readonly IServiceProvider _serviceProvider = TestServiceProviderFactory.Create(options =>
    {
        options.ConfigureConfiguration(builder =>
        {
            builder.AddNode<ComponentMock>("Parent", "/primary-parent-uri", n =>
            {
                n.WithUris("/secondary-parent-uri");
                n.AddNode("Child_1");
                n.AddNode("Child_2", n =>
                {
                    n.AddNode<ComponentMock>("Child_2_A", "/nested/child-a");
                    n.AddNode<OtherComponentMock>("Child_2_B", "/nested/child-b");
                });
                n.AddNode<OtherComponentMock>("Child_3", "/child-3");
            });
        });
    });

    private IRoutingConfiguration RoutingConfiguration => _serviceProvider.GetRequiredService<IRoutingConfigurationProvider>().GetRoutingConfiguration();

    [Fact]
    public void HasUri_ReturnsTrue()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var hasUri = parentNode.HasUri("/secondary-parent-uri");
        Assert.True(hasUri);
    }

    [Fact]
    public void FindChildOrItselfByKey_FindsChild()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var childNode = parentNode.FindChildOrItselfByKey("Child_1");
        Assert.NotNull(childNode);
    }

    [Fact]
    public void FindChildOrItselfByKey_FindsNestedChild()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var nestedChildNode = parentNode.FindChildOrItselfByKey("Child_2_B");
        Assert.NotNull(nestedChildNode);
    }

    [Fact]
    public void FindChildOrItselfByKey_FindsItself()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var itself = parentNode.FindChildOrItselfByKey("Parent");
        Assert.NotNull(itself);
    }

    [Fact]
    public void FindChildOrItselfByUri_FindsChild()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var childNode = parentNode.FindChildOrItselfByUri("/child-3");
        Assert.NotNull(childNode);
    }

    [Fact]
    public void FindChildOrItselfByUri_FindsNestedChild()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var nestedChildNode = parentNode.FindChildOrItselfByUri("/nested/child-a");
        Assert.NotNull(nestedChildNode);
    }

    [Fact]
    public void FindChildOrItselfByUri_FindsItself()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var itself = parentNode.FindChildOrItselfByUri("/primary-parent-uri");
        Assert.NotNull(itself);
    }

    [Fact]
    public void FindChildrenAndOrItselfByComponentType_FindsChildrenAndItself()
    {
        var parentNode = RoutingConfiguration.NodesInHierarchy.Single();

        var nodesWithComponent = parentNode.FindChildrenAndOrItselfByComponentType(typeof(ComponentMock));
        Assert.Equal(2, nodesWithComponent.Count);
        Assert.Contains(nodesWithComponent, n => n.Key == "Parent");
        Assert.Contains(nodesWithComponent, n => n.Key == "Child_2_A");
    }
}
