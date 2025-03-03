﻿using System.Reflection;
using Trailblazor.Routing.Configuration;

namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Options for the <see cref="TrailblazorRouter"/> and adjacent services.
/// </summary>
public interface IRoutingOptions
{
    /// <summary>
    /// Action allowing to configure the <see cref="IRoutingConfiguration"/> when registering Trailblazor Routing. This action will
    /// be invoked only once after resolving and running through the <see cref="IRoutingProfile"/>s.
    /// </summary>
    public Action<IRoutingConfigurationBuilder>? ProfileAction { get; }

    /// <summary>
    /// Registered types implementing the <see cref="IRoutingProfile"/> interface. These types will be resolved once to configure the <see cref="IRoutingConfiguration"/>.
    /// </summary>
    public IReadOnlyList<Type> RoutingProfileTypes { get; }

    /// <summary>
    /// Assemblies that are to be scanned for nodes.
    /// </summary>
    public IReadOnlyList<Assembly> NodeScanAssemblies { get; }

    /// <summary>
    /// Determines whether the fully configured <see cref="IRoutingConfiguration"/> is to be validated or not.
    /// </summary>
    public bool RoutingConfigurationValidationDisabled { get; }
}
