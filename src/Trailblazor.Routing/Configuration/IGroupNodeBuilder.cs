using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Builder constructs an instance of <see cref="IGroupNode"/>.
/// </summary>
public interface IGroupNodeBuilder
{
    /// <summary>
    /// Adds a node instance to the group node that is currently being configured.
    /// </summary>
    /// <param name="key">Key of the node instance.</param>
    /// <param name="node">The node instance to be added.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public IGroupNodeBuilder AddNode(string key, INode node);

    /// <summary>
    /// Adds a child group to the group node that is currently being configured.
    /// </summary>
    /// <param name="key">Key of the child group node.</param>
    /// <param name="group">Optional builder action for configuring the child group.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public IGroupNodeBuilder AddNode(string key, Action<IGroupNodeBuilder>? group = null);

    /// <summary>
    /// Adds a route to the group.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="key">Key of the route node.</param>
    /// <param name="uri">Relative URI of the route.</param>
    /// <param name="route">Builder action for further configuring the route.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public IGroupNodeBuilder AddNode<TComponent>(string key, string uri, Action<IRouteNodeBuilder>? route = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Adds a route to the group node.
    /// </summary>
    /// <param name="key">Key of the route node.</param>
    /// <param name="uri">Relative URI of the route.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <param name="route">Builder action for further configuring the route.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public IGroupNodeBuilder AddNode(string key, string uri, Type componentType, Action<IRouteNodeBuilder>? route = null);

    /// <summary>
    /// Configures a route that represents the group node. 
    /// </summary>
    /// <remarks>
    /// This route will be used as the route associated with the group node. This route is completely optional.
    /// </remarks>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="key">Key of the route node.</param>
    /// <param name="uri">Relative URI of the route.</param>
    /// <param name="route">Builder action for further configuring the route.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public IGroupNodeBuilder RepresentedByRouteNode<TComponent>(string key, string uri, Action<IRouteNodeBuilder>? route = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Configures a route that represents the group node. 
    /// </summary>
    /// <remarks>
    /// This route will be used as the route associated with the group node. This route is completely optional.
    /// </remarks>
    /// <param name="key">Key of the route node.</param>
    /// <param name="uri">Relative URI of the route.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <param name="route">Builder action for further configuring the route.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public IGroupNodeBuilder RepresentedByRouteNode(string key, string uri, Type componentType, Action<IRouteNodeBuilder>? route = null);

    /// <summary>
    /// Adds metadata to the group.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public IGroupNodeBuilder WithMetadata(string key, object value);

    /// <summary>
    /// Completes the configuration process and builds the configured <see cref="IGroupNode"/>.
    /// </summary>
    /// <returns>The configured <see cref="IGroupNode"/>.</returns>
    internal IGroupNode Build();
}
