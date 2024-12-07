namespace Trailblazor.Routing.Parsing;

/// <summary>
/// Service handles parsing tasks for components.
/// </summary>
public interface IComponentParameterParser
{
    /// <summary>
    /// Method parses given <paramref name="unparsedComponentParameters"/> to the parameter values of the given <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component whose parameters are to be set.</param>
    /// <param name="unparsedComponentParameters">Query parameters of key value pairs.</param>
    /// <returns>Dictionary of parsed parameter key values.</returns>
    public Dictionary<string, object> GetComponentParametersFromQueryParameters(Type componentType, Dictionary<string, string> unparsedComponentParameters);
}
