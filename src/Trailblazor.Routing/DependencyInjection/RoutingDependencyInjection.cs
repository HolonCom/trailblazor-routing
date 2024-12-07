using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Configuration.Validation;
using Trailblazor.Routing.Parsing;

namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Static class that contains routing related extension methods.
/// </summary>
public static class RoutingDependencyInjection
{
    /// <summary>
    /// Adds services required for the <see cref="TrailblazorRouter"/> to function correctly to the given <paramref name="services"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> being extended and gettint Trailblazor services registered to it.</param>
    /// <param name="options"> Optional builder action for configuring <see cref="IRoutingOptions"/>. The configured options can later be accessed by injecting the <see cref="IRoutingOptionsProvider"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> for further dependency injection configurations.</returns>
    public static IServiceCollection AddTrailblazorRouting(this IServiceCollection services, Action<IRoutingOptionsBuilder>? options = null)
    {
        var optionsBuilder = new RoutingOptionsBuilder();
        options?.Invoke(optionsBuilder);
        var routingOptions = optionsBuilder.Build();

        services.AddSingleton<IRoutingOptionsProvider>(sp => new RoutingOptionsProvider(routingOptions));
        services.AddSingleton<IRoutingConfigurationValidator, RoutingConfigurationValidator>();
        services.AddScoped<INodeResolver, RouteNodeResolver>();
        services.AddScoped<IRoutingConfigurationResolver, RoutingConfigurationResolver>();
        services.AddScoped<IRoutingConfigurationProvider, RoutingConfigurationProvider>();
        services.AddScoped<INodeProvider, NodeProvider>();
        services.AddScoped<IRouterContextProvider, RouterContextProvider>();
        services.AddSingleton<IUriParser, Parsing.UriParser>();
        services.AddSingleton<IComponentParameterParser, ComponentParameterParser>();

        foreach (var routingProfileType in routingOptions.RoutingProfileTypes)
            services.AddScoped(typeof(IRoutingProfile), routingProfileType);

        return services;
    }
}
