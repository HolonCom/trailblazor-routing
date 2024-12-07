using System.Reflection;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Exceptions;

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

    public IRoutingOptionsBuilder ScanForNodesInAssemblies(params List<Assembly> assemblies)
    {
        _routingOptions.InternalNodeScanAssemblies = _routingOptions.NodeScanAssemblies.Concat(assemblies).Distinct().ToList();
        return this;
    }

    public IRoutingOptionsBuilder DisableRoutingConfigurationValidation()
    {
        _routingOptions.RoutingConfigurationValidationDisabled = true;
        return this;
    }

    public IRoutingOptions Build()
    {
        return _routingOptions;
    }
}
