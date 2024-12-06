using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Builder for the <see cref="IRoutingConfiguration"/>.
/// </summary>
public interface IRoutingConfigurationBuilder
{
    /// <summary>
    /// Adds a node instance to the configuration.
    /// </summary>
    /// <param name="node">Instance of a node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNode(INode node);

    /// <summary>
    /// Adds a node to the configuration.
    /// </summary>
    /// <param name="key">Key of the child node.</param>
    /// <param name="builder">Optional builder action for configuring the child node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNode(string key, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Adds a route node to the configuration.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route node.</typeparam>
    /// <param name="key">Unique key of the route.</param>
    /// <param name="uri">Unique relative URI of the route.</param>
    /// <param name="builder">Builder action for further configuring the route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNode<TComponent>(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Adds a route node to the configuration.
    /// </summary>
    /// <param name="key">Unique key of the route.</param>
    /// <param name="uri">Unique relative URI of the route.</param>
    /// <param name="componentType">Type of component representing the route node.</param>
    /// <param name="builder">Builder action for further configuring the route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNode(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Adds an instance of a node to the node with the <paramref name="targetNodeKey"/>.
    /// </summary>
    /// <param name="targetNodeKey">Key of the target node.</param>
    /// <param name="node">Instance of a node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetNodeKey, INode node);

    /// <summary>
    /// Adds a new node to the node with the given <paramref name="targetNodeKey"/>.
    /// </summary>
    /// <param name="targetNodeKey">Key of the target parent node.</param>
    /// <param name="key">Key of the node.</param>
    /// <param name="builder">Builder action to further configure the new node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetNodeKey, string key, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Adds a new route to the node with the given <paramref name="targetNodeKey"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="targetNodeKey">Key of the target parent node.</param>
    /// <param name="key">Key of the route.</param>
    /// <param name="uri">URI of the route.</param>
    /// <param name="builder">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNodeToNode<TComponent>(string targetNodeKey, string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Adds a new route to the node with the given <paramref name="targetNodeKey"/>.
    /// </summary>
    /// <param name="targetNodeKey">Key of the target parent node.</param>
    /// <param name="key">Key of the route.</param>
    /// <param name="uri">URI of the route.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <param name="builder">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddNodeToNode(string targetNodeKey, string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Replaces the node with the given <paramref name="targetNodeKey"/> with the <paramref name="node"/> instance.
    /// </summary>
    /// <param name="targetNodeKey">Key of the node that is to be replaced.</param>
    /// <param name="node">Node instance replacing the target node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, INode node);

    /// <summary>
    /// Replaces the node with the given <paramref name="targetNodeKey"/> with the node configured using the <paramref name="builder"/> builder action.
    /// </summary>
    /// <param name="targetNodeKey">Key of the node that is to be replaced.</param>
    /// <param name="builder">Builder action for configuring the node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, Action<INodeBuilder> builder);

    /// <summary>
    /// Replaces the route with the given <paramref name="targetNodeKey"/> with the newly configured route.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="targetNodeKey">Key of the route.</param>
    /// <param name="uri">URI of the route.</param>
    /// <param name="builder">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceNode<TComponent>(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Replaces the route with the given <paramref name="targetNodeKey"/> with the newly configured route.
    /// </summary>
    /// <param name="targetNodeKey">Key of the route that is being replaced.</param>
    /// <param name="uri">URI of the route.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <param name="builder">Builder action for further configuring the new route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceNode(string targetNodeKey, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Replaces the representing component of a route with the given <paramref name="key"/> with the specified <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="key">Key of the route whose component is to be swapped.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceNodeComponent<TComponent>(string key)
        where TComponent : ComponentBase;

    /// <summary>
    /// Replaces the representing component of a route with the given <paramref name="key"/> with the specified <paramref name="componentType"/>.
    /// </summary>
    /// <param name="key">Key of the route whose component is to be swapped.</param>
    /// <param name="componentType">Type of component representing the route.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder ReplaceNodeComponent(string key, Type componentType);

    /// <summary>
    /// Moves the node with the given <paramref name="nodeKey"/> to the node with the given <paramref name="targetNodeKey"/>.
    /// </summary>
    /// <param name="nodeKey">Key of the node that is to be moved.</param>
    /// <param name="targetNodeKey">Key of the target parent node.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder MoveNodeToNode(string nodeKey, string targetNodeKey);

    /// <summary>
    /// Removes the node with the given <paramref name="key"/> from the configuration, if it has been found..
    /// </summary>
    /// <param name="key">Key of the node that is to be deleted.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder RemoveNode(string key);

    /// <summary>
    /// Adds metadata to the node with the given  <paramref name="targetNodeKey"/>.
    /// </summary>
    /// <param name="targetNodeKey">Key of the node, the metadata is to be atted to.</param>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder AddMetadataToNode(string targetNodeKey, string key, object value);

    /// <summary>
    /// Sets the <paramref name="uri"/> that is to be redirected to, if no route for the current URI was found.
    /// </summary>
    /// <param name="uri">URI to redirect to if not route node for the current URI was found.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder RedirectOnNotFound([StringSyntax(StringSyntaxAttribute.Uri)] string uri);

    /// <summary>
    /// Sets the component that is to be rendered if no route for the current URI was found.
    /// </summary>
    /// <remarks>
    /// This will be ignored if a <see cref="IRoutingConfiguration.NotFoundRedirectUri"/> was defined.
    /// </remarks>
    /// <typeparam name="TComponent">Type of component to be rendered.</typeparam>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder UseComponentOnNotFound<TComponent>()
        where TComponent : ComponentBase;

    /// <summary>
    /// Sets the component that is to be rendered if no route for the current URI was found.
    /// </summary>
    /// <remarks>
    /// This will be ignored if a <see cref="IRoutingConfiguration.NotFoundRedirectUri"/> was defined.
    /// </remarks>
    /// <param name="componentType">Type of component to be rendered.</param>
    /// <returns>The <see cref="IRoutingConfigurationBuilder"/> for further configurations.</returns>
    public IRoutingConfigurationBuilder UseComponentOnNotFound(Type componentType);

    /// <summary>
    /// Completes the configuration process and builds the configured <see cref="IRoutingConfiguration"/>.
    /// </summary>
    /// <returns>The configured <see cref="IRoutingConfiguration"/>.</returns>
    internal IRoutingConfiguration Build();
}
