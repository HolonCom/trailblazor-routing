using System.Reflection;
using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing.DependencyInjection;

internal sealed class RoutingOptions : IRoutingOptions
{
    internal List<Type> InternalRoutingProfileTypes { get; } = [];
    internal List<Assembly> InternalNodeScanAssemblies { get; set; } = [];

    public Action<IRoutingConfigurationBuilder>? ProfileAction { get; internal set; }
    public IReadOnlyList<Type> RoutingProfileTypes => InternalRoutingProfileTypes;
    public IReadOnlyList<Assembly> NodeScanAssemblies => InternalNodeScanAssemblies;
    public bool RoutingConfigurationValidationDisabled { get; internal set; }
}
