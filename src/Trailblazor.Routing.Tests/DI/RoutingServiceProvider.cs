using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.Tests.DI;

internal static class TestServiceProviderFactory
{
    internal static IServiceProvider Create(Action<IRoutingOptionsBuilder>? options = null)
    {
        return new ServiceCollection().AddTrailblazorRouting(options).BuildServiceProvider();
    }

    internal static IServiceProvider Create(Action<IRoutingConfigurationBuilder> builder)
    {
        return new ServiceCollection().AddTrailblazorRouting(options => options.ConfigureConfiguration(builder)).BuildServiceProvider();
    }
}
