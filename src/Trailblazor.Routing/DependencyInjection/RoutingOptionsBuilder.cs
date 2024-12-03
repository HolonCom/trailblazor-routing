using System.Reflection;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Configuration.Validation;

namespace Trailblazor.Routing.DependencyInjection;

internal sealed class RoutingOptionsBuilder : IRoutingOptionsBuilder
{
    private readonly RoutingOptions _routingOptions = new();

    public IRoutingOptionsBuilder AddProfile<TProfile>()
        where TProfile : class, IRoutingProfile
    {
        return AddProfile(typeof(TProfile));
    }

    public IRoutingOptionsBuilder AddProfile(Type type)
    {
        RoutingValidationException.ThrowIfTypeIsNotAProfile(type);

        _routingOptions.InternalRoutingProfileTypes.Add(type);
        return this;
    }

    public IRoutingOptionsBuilder AddProfilesFromAssemblies(params List<Assembly> assemblies)
    {
        var routingProfileTypes = assemblies.SelectMany(a => a.GetTypes()).Where(t => !t.IsAbstract && !t.IsValueType && !t.IsInterface && t.IsAssignableTo(typeof(IRoutingProfile)));
        foreach (var routingProfileType in routingProfileTypes)
            AddProfile(routingProfileType);

        return this;
    }

    public IRoutingOptionsBuilder ConfigureConfiguration(Action<IRoutingConfigurationBuilder> configuration)
    {
        _routingOptions.ProfileAction = configuration;
        return this;
    }

    public IRoutingOptions Build()
    {
        return _routingOptions;
    }
}
