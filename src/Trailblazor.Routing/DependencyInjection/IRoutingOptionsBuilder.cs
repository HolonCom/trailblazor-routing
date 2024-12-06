using System.Reflection;
using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Builder for configuring <see cref="IRoutingOptions"/>.
/// </summary>
public interface IRoutingOptionsBuilder
{
    /// <summary>
    /// Adds the specified <typeparamref name="TProfile"/> as an implementation of <see cref="IRoutingProfile"/> to the options.
    /// </summary>
    /// <typeparam name="TProfile">Type of <see cref="IRoutingProfile"/> implementation.</typeparam>
    /// <returns>The <see cref="IRoutingOptionsBuilder"/> for further configurations.</returns>
    public IRoutingOptionsBuilder AddProfile<TProfile>()
        where TProfile : class, IRoutingProfile;

    /// <summary>
    /// Adds the specified <paramref name="type"/> as an implementation of <see cref="IRoutingProfile"/> to the options.
    /// </summary>
    /// <param name="type">Type of <see cref="IRoutingProfile"/> implementation.</param>
    /// <returns>The <see cref="IRoutingOptionsBuilder"/> for further configurations.</returns>
    public IRoutingOptionsBuilder AddProfile(Type type);

    /// <summary>
    /// Scans for <see cref="IRoutingProfile"/> implementations in the given <paramref name="assemblies"/> and registers them.
    /// </summary>
    /// <param name="assemblies">Assemblies to be scanned for <see cref="IRoutingProfile"/>s.</param>
    /// <returns>The <see cref="IRoutingOptionsBuilder"/> for further configurations.</returns>
    public IRoutingOptionsBuilder AddProfilesFromAssemblies(params List<Assembly> assemblies);

    /// <summary>
    /// Allows configuring the <see cref="IRoutingConfiguration"/> using a builder action.
    /// </summary>
    /// <param name="configuration">Builder action for configuring the <see cref="IRoutingConfiguration"/> outside
    /// of <see cref="IRoutingProfile"/>s when registering Trailblazor Routing.</param>
    /// <returns>The <see cref="IRoutingOptionsBuilder"/> for further configurations.</returns>
    public IRoutingOptionsBuilder ConfigureConfiguration(Action<IRoutingConfigurationBuilder> configuration);

    /// <summary>
    /// Scans for components in the given <paramref name="assemblies"/> and adds them as top-level <see cref="INode"/>s.
    /// </summary>
    /// <param name="assemblies">Assemblies to scan in.</param>
    /// <returns></returns>
    public IRoutingOptionsBuilder ScanForNodesInAssemblies(params List<Assembly> assemblies);

    /// <summary>
    /// Completes the configuration process and builds the configured <see cref="IRoutingOptions"/>.
    /// </summary>
    /// <returns>The configured <see cref="IRoutingOptions"/>.</returns>
    internal IRoutingOptions Build();
}
