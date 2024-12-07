using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.Tests.DI;

public static class TestServiceProviderFactory
{
    public static IServiceProvider Create(Action<IRoutingOptionsBuilder>? options = null)
    {
        return new ServiceCollection().AddTrailblazorRouting(options).BuildServiceProvider();
    }
}
