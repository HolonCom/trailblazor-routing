namespace Trailblazor.Routing;

/// <summary>
/// Attribute configures a component types key.
/// </summary>
/// <remarks>
/// Sets the <see cref="NodeKey"/> using the given <paramref name="nodeKey"/>.
/// </remarks>
/// <param name="nodeKey">Key of the node.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class NodeKeyAttribute(string nodeKey) : Attribute
{
    /// <summary>
    /// Key of the node.
    /// </summary>
    public string NodeKey { get; } = nodeKey;
}
