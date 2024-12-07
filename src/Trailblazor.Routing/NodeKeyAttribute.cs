namespace Trailblazor.Routing;

/// <summary>
/// Attribute configures a component types key.
/// </summary>
/// <remarks>
/// This attribute is not mandatory. If not specified, the name of the component type is being used as the node key.
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
