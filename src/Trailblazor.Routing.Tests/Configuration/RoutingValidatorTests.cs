using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Tests.DI;
using Trailblazor.Routing.Tests.Mocks;

namespace Trailblazor.Routing.Tests.Configuration;

public class RoutingValidatorTests
{
    private readonly Func<IServiceProvider, IRoutingConfiguration> _validateConfigurationAction = (sp) => sp.GetRequiredService<IRoutingConfigurationProvider>().GetRoutingConfiguration();

    [Fact]
    public void ValidateAndThrowIfInvalid_DoesntThrowAndFlattenedNodesAreComplete()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("ParentNode", n => n
                .AddNode("ChildNode", m => m
                    .AddNode("NestedChildNode")));
        });

        try
        {
            var routingConfiguration = _validateConfigurationAction(serviceProvider);
            Assert.Equal(3, routingConfiguration.FlattenedNodes.Count);
        }
        catch (RoutingValidationException ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [Fact]
    public void ValidateAndThrowIfInvalid_DuplicateKeys_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("ParentNode", n => n.AddNode("ParentNode"));
        });

        Assert.Throws<RoutingValidationException>(() => _validateConfigurationAction(serviceProvider));
    }

    [Fact]
    public void ValidateAndThrowIfInvalid_DuplicateUris_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode<ComponentMock>("ParentNode", "/parent");
            builder.AddNode<ComponentMock>("AnotherParentNode", "/parent");
        });

        Assert.Throws<RoutingValidationException>(() => _validateConfigurationAction(serviceProvider));
    }

    [Fact]
    public void ValidateAndThrowIfInvalid_NodesWithUrisButWithoutComponentTypes_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("ParentNode", "/parent", null!);
        });

        Assert.Throws<RoutingValidationException>(() => _validateConfigurationAction(serviceProvider));
    }

    [Fact]
    public void ValidateAndThrowIfInvalid_NodesWithUrisAndComponentTypes_DoesntThrow()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode<ComponentMock>("ParentNode", "/parent");
        });

        try
        {
            _validateConfigurationAction(serviceProvider);
        }
        catch (RoutingValidationException ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [Fact]
    public void ValidateAndThrowIfInvalid_NodesWithNonComponentTypes_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("ParentNode", "/parent", typeof(RoutingValidatorTests));
        });

        Assert.Throws<RoutingValidationException>(() => _validateConfigurationAction(serviceProvider));
    }

    [Fact]
    public void ValidateAndThrowIfInvalid_NodesWithCorrectRelationships_DoesntThrow()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("ParentNode");
            builder.AddNodeToNode("ParentNode", "ChildNode");
        });

        try
        {
            _validateConfigurationAction(serviceProvider);
        }
        catch (RoutingValidationException ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [Fact]
    public void ValidateAndThrowIfInvalid_NotFoundComponentTypeIsNoComponent_Throws()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.UseComponentOnNotFound(typeof(RoutingValidatorTests));
        });

        Assert.Throws<RoutingValidationException>(() => _validateConfigurationAction(serviceProvider));
    }
}
