using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Builder for the <see cref="IRoutingConfiguration"/>.
/// </summary>
/// <remarks>
/// TODO -> Document the order of the actions!
/// </remarks>
public interface IRoutingConfigurationBuilder
{
    /// <summary>
    /// Adds a group node to the configuration.
    /// </summary>
    /// <param name="key">Key of the child group node.</param>
    /// <param name="group">Optional builder action for configuring the child group.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddGroup(string key, Action<IGroupNodeBuilder>? group = null);

    /// <summary>
    /// Adds a new group to the group with the given <paramref name="targetGroupKey"/>.
    /// </summary>
    /// <param name="targetGroupKey">Key of the target parent group.</param>
    /// <param name="groupKey">Key of the group.</param>
    /// <param name="group">Builder action to further configure the new group.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddGroupToGroup(string targetGroupKey, string groupKey, Action<IGroupNodeBuilder>? group = null);

    /// <summary>
    /// Adds a new route to the group with the given <paramref name="targetGroupKey"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="targetGroupKey">Key of the target parent group.</param>
    /// <param name="routeKey">Key of the route.</param>
    /// <param name="routeUri">URI of the route.</param>
    /// <param name="route">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddRouteToGroup<TComponent>(string targetGroupKey, string routeKey, string routeUri, Action<IRouteNodeBuilder>? route = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Adds a new route to the group with the given <paramref name="targetGroupKey"/>.
    /// </summary>
    /// <param name="targetGroupKey">Key of the target parent group.</param>
    /// <param name="routeKey">Key of the route.</param>
    /// <param name="routeUri">URI of the route.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <param name="route">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddRouteToGroup(string targetGroupKey, string routeKey, string routeUri, Type componentType, Action<IRouteNodeBuilder>? route = null);

    /// <summary>
    /// Replaces the group node with the given <paramref name="groupKey"/> with the group node configured using the <paramref name="group"/> builder action.
    /// </summary>
    /// <param name="groupKey">Key of the group that is to be replaced.</param>
    /// <param name="group">Builder action for configuring the group node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceGroupNode(string groupKey, Action<IGroupNodeBuilder> group);

    /// <summary>
    /// Replaces the route with the given <paramref name="routeKey"/> with the newly configured route.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="routeKey">Key of the route.</param>
    /// <param name="routeUri">URI of the route.</param>
    /// <param name="route">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceRouteNode<TComponent>(string routeKey, string routeUri, Action<IRouteNodeBuilder>? route = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Replaces the route with the given <paramref name="routeKey"/> with the newly configured route.
    /// </summary>
    /// <param name="routeKey">Key of the route that is being replaced.</param>
    /// <param name="routeUri">URI of the route.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <param name="route">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceRouteNode(string routeKey, string routeUri, Type componentType, Action<IRouteNodeBuilder>? route = null);

    /// <summary>
    /// Replaces the representing component of a route with the given <paramref name="key"/> with the specified <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="key">Key of the route whose component is to be swapped.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceRouteNodeComponent<TComponent>(string key)
        where TComponent : ComponentBase;

    /// <summary>
    /// Replaces the representing component of a route with the given <paramref name="key"/> with the specified <paramref name="componentType"/>.
    /// </summary>
    /// <param name="key">Key of the route whose component is to be swapped.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceRouteNodeComponent(string key, Type componentType);

    /// <summary>
    /// Moves the node with the given <paramref name="nodeKey"/> to the group node with the given <paramref name="targetGroupKey"/>.
    /// </summary>
    /// <param name="nodeKey">Key of the node that is to be moved.</param>
    /// <param name="targetGroupKey">Key of the target parent group node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder MoveNode(string nodeKey, string targetGroupKey);

    /// <summary>
    /// Removes the node with the given <paramref name="key"/> from the configuration, if it has been found..
    /// </summary>
    /// <param name="key">Key of the node that is to be deleted.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder RemoveNode(string key);

    /// <summary>
    /// Adds metadata to the node with the given  <paramref name="nodeKey"/>.
    /// </summary>
    /// <param name="nodeKey">Key of the node, the metadata is to be atted to.</param>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddMetadataToNode(string nodeKey, string key, object value);

    /// <summary>
    /// Completes the configuration process and builds the configured <see cref="IRoutingConfiguration"/>.
    /// </summary>
    /// <returns>The configured <see cref="IRoutingConfiguration"/>.</returns>
    internal IRoutingConfiguration Build();
}
