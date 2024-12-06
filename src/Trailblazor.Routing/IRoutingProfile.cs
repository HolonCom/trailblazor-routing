using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing;

/// <summary>
/// Routing profile for configuring the <see cref="IRoutingConfiguration"/> used by Trailblazor Routing outside of the
/// dependency injection registration. Doing this allows for more flexibility.
/// </summary>
public interface IRoutingProfile
{
    /// <summary>
    /// Configures an <see cref="IRoutingConfiguration"/> using the given <paramref name="builder"/> action.
    /// </summary>
    /// <param name="builder">Builder action for configuring <see cref="IRoutingConfiguration"/>.</param>
    public void ConfigureRoutes(IRoutingConfigurationBuilder builder);
}
