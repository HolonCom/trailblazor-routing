using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing.DependencyInjection;

internal sealed class RoutingOptions : IRoutingOptions
{
    internal List<Type> InternalRoutingProfileTypes { get; } = [];

    public Action<IRoutingConfigurationBuilder>? ProfileAction { get; internal set; }
    public IReadOnlyList<Type> RoutingProfileTypes => InternalRoutingProfileTypes;
}
