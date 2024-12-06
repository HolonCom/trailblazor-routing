namespace Trailblazor.Routing;

/// <summary>
/// Attribute configures a component types parent node.
/// </summary>
/// <remarks>
/// Sets the <see cref="ParentNodeKey"/> using the given <paramref name="parentNodeKey"/>.
/// </remarks>
/// <param name="parentNodeKey">Key of the parent node.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class NodeParentAttribute(string parentNodeKey) : Attribute
{
    /// <summary>
    /// Key of the parent node.
    /// </summary>
    public string ParentNodeKey { get; } = parentNodeKey;
}
