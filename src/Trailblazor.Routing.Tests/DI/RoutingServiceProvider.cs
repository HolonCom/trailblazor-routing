using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.Tests.DI;

public static class TestServiceProviderFactory
{
    public static IServiceProvider Create(Action<IRoutingOptionsBuilder>? options = null)
    {
        return new ServiceCollection().AddTrailblazorRouting(options).BuildServiceProvider();
    }

    public static IServiceProvider Create(Action<IRoutingConfigurationBuilder> builder)
    {
        return new ServiceCollection().AddTrailblazorRouting(options => options.ConfigureConfiguration(builder)).BuildServiceProvider();
    }
}
