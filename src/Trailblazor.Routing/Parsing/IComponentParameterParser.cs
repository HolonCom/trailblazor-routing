namespace Trailblazor.Routing.Parsing;

/// <summary>
/// Service handles parsing tasks for components.
/// </summary>
internal interface IComponentParameterParser
{
    /// <summary>
    /// Method parses given <paramref name="unparserComponentParameters"/> to the parameter values of the given <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component whose parameters are to be set.</param>
    /// <param name="unparserComponentParameters">Query parameters of key value pairs.</param>
    /// <returns>Dictionary of parsed parameter key values.</returns>
    internal Dictionary<string, object> GetComponentParametersFromQueryParameters(Type componentType, Dictionary<string, string> unparserComponentParameters);
}
