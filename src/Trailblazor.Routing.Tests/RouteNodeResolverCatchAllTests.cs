using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Tests.DI;
using Trailblazor.Routing.Tests.Mocks;

namespace Trailblazor.Routing.Tests;

public class RouteNodeResolverCatchAllTests
{
    private const string BaseRoute = "nl/downloads/2627";
    private const string CatchAllRoute = "nl/downloads/2627/{*Path}";
    private const string SingleSegmentRoute = "nl/{CmsPath}";

    private static INodeResolver CreateResolver()
    {
        var serviceProvider = TestServiceProviderFactory.Create(builder =>
        {
            builder.AddNode("Base", BaseRoute, typeof(CatchAllComponentMock));
            builder.AddNode("CatchAll", CatchAllRoute, typeof(CatchAllComponentMock));
            builder.AddNode("Cms", SingleSegmentRoute, typeof(ComponentMock));
        });

        return serviceProvider.GetRequiredService<INodeResolver>();
    }

    [Fact]
    public void ResolveNodeForUri_CatchAll_BindsRemainingPathIncludingSlashes()
    {
        var result = CreateResolver().ResolveNodeForUri("/nl/downloads/2627/a/b");

        Assert.NotNull(result.Node);
        Assert.Equal("CatchAll", result.Node!.Key);
        Assert.Equal("a/b", result.ComponentParameters[nameof(CatchAllComponentMock.Path)]);
    }

    [Fact]
    public void ResolveNodeForUri_CatchAll_SingleSegment_BindsSegment()
    {
        var result = CreateResolver().ResolveNodeForUri("/nl/downloads/2627/sub");

        Assert.Equal("CatchAll", result.Node?.Key);
        Assert.Equal("sub", result.ComponentParameters[nameof(CatchAllComponentMock.Path)]);
    }

    [Fact]
    public void ResolveNodeForUri_CatchAll_UnescapesSegments()
    {
        var result = CreateResolver().ResolveNodeForUri("/nl/downloads/2627/seizoen%20a/map%201");

        Assert.Equal("CatchAll", result.Node?.Key);
        Assert.Equal("seizoen a/map 1", result.ComponentParameters[nameof(CatchAllComponentMock.Path)]);
    }

    [Fact]
    public void ResolveNodeForUri_BaseRoute_DoesNotMatchCatchAll()
    {
        var result = CreateResolver().ResolveNodeForUri("/nl/downloads/2627");

        Assert.Equal("Base", result.Node?.Key);
        Assert.False(result.ComponentParameters.ContainsKey(nameof(CatchAllComponentMock.Path)));
    }

    [Fact]
    public void ResolveNodeForUri_CatchAll_IgnoresQueryString()
    {
        var result = CreateResolver().ResolveNodeForUri("/nl/downloads/2627/a/b?target=new");

        Assert.Equal("CatchAll", result.Node?.Key);
        Assert.Equal("a/b", result.ComponentParameters[nameof(CatchAllComponentMock.Path)]);
    }

    [Fact]
    public void ResolveNodeForUri_SingleSegmentParameter_StillDoesNotMatchMultiSegment()
    {
        var result = CreateResolver().ResolveNodeForUri("/nl/other/deeper");

        Assert.Null(result.Node);
    }

    [Fact]
    public void ResolveNodeForUri_SingleSegmentParameter_MatchesSingleSegment()
    {
        var result = CreateResolver().ResolveNodeForUri("/nl/other");

        Assert.Equal("Cms", result.Node?.Key);
    }
}
