using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Builder for configuring an <see cref="INode"/>.
/// </summary>
public interface INodeBuilder
{
    /// <summary>
    /// Adds an additional <paramref name="uri"/> to the node.
    /// </summary>
    /// <remarks>
    /// This <paramref name="uri"/> obivously has to be unique.
    /// </remarks>
    /// <param name="uri">Additional URI to the node.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public INodeBuilder WithUri([StringSyntax(StringSyntaxAttribute.Uri)] string uri);

    /// <summary>
    /// Adds a node.
    /// </summary>
    /// <param name="key">Key of the node instance.</param>
    /// <param name="node">The node instance to be added.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public INodeBuilder AddNode(string key, INode node);

    /// <summary>
    /// Adds a node.
    /// </summary>
    /// <param name="key">Key of the node.</param>
    /// <param name="builder">Builder action for further configuring the node.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public INodeBuilder AddNode(string key, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Adds a node with a <paramref name="uri"/> and a <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the node.</typeparam>
    /// <param name="key">Key of the node.</param>
    /// <param name="uri">Relative URI of the node.</param>
    /// <param name="builder">Builder action for further configuring the node.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public INodeBuilder AddNode<TComponent>(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Action<INodeBuilder>? builder = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Adds a node with a <paramref name="uri"/> and a <paramref name="componentType"/>.
    /// </summary>
    /// <param name="key">Key of the node.</param>
    /// <param name="uri">Relative URI of the node.</param>
    /// <param name="componentType">Type of component representing the node.</param>
    /// <param name="builder">Builder action for further configuring the node.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public INodeBuilder AddNode(string key, [StringSyntax(StringSyntaxAttribute.Uri)] string uri, Type componentType, Action<INodeBuilder>? builder = null);

    /// <summary>
    /// Adds metadata to the node.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>The current builder for further group configurations.</returns>
    public INodeBuilder WithMetadata(string key, object value);

    /// <summary>
    /// Completes the configuration process and builds the configured <see cref="INode"/>.
    /// </summary>
    /// <returns>The configured <see cref="INode"/>.</returns>
    internal INode Build();
}
