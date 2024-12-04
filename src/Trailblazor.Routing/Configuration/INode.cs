namespace Trailblazor.Routing.Configuration;

/// <summary>
/// Node in a routing configuration.
/// </summary>
public interface INode
{
    /// <summary>
    /// Unique key of the node.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Metadata of the node. Allows for adding and removing metadata values (such as a 'hidden'-flag for a menu) to and from a node.
    /// </summary>
    public IReadOnlyDictionary<string, object?> Metadata { get; }

    /// <summary>
    /// Method fetches the nodes metadata value for the specified <paramref name="key"/> and casts it into the <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of metadata value.</typeparam>
    /// <param name="key">Key of the desired value.</param>
    /// <param name="defaultValue">Optional default value in case the desired value has not been found.</param>
    /// <returns>Found metadata value for the specified <paramref name="key"/>.</returns>
    /// <exception cref="InvalidCastException">Thrown if the fetched value is not of type <typeparamref name="TValue"/>.</exception>
    public TValue? GetMetadataValue<TValue>(string key, TValue? defaultValue = default);

    /// <summary>
    /// Method fetches the nodes metadata value for the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the desired value.</param>
    /// <param name="defaultValue">Optional default value in case the desired value has not been found.</param>
    /// <returns>Found metadata value for the specified <paramref name="key"/>.</returns>
    public object? GetMetadataValue(string key, object? defaultValue = default);

    /// <summary>
    /// Method sets the metadata <paramref name="value"/> for the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the <paramref name="value"/> to be set.</param>
    /// <param name="value">Value to be set.</param>
    public void SetMetadataValue(string key, object? value);

    /// <summary>
    /// Method removes the nodes metadata value with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the metadata value that is to be removed.</param>
    public void RemoveMetadataValue(string key);

    /// <summary>
    /// Method determines whether the node has a metadata value with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key that is to be checked for.</param>
    /// <returns><see langword="true"/> if the nodes metadata contains a value with the specified <paramref name="key"/>.</returns>
    public bool HasMetadataValue(string key);
}
