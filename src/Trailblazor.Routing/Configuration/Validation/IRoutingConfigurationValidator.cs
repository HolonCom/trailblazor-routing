namespace Trailblazor.Routing.Configuration.Validation;

/// <summary>
/// Service validates a given <see cref="IRoutingConfiguration"/> for potential problems.
/// </summary>
internal interface IRoutingConfigurationValidator
{
    /// <summary>
    /// Method validates the given <paramref name="routingConfiguration"/> and throws a <see cref="RoutingValidationException"/> if
    /// a validation error is detected.
    /// </summary>
    /// <param name="routingConfiguration">Routing configuration to be validated.</param>
    public void ValidateAndThrowIfInvalid(IRoutingConfiguration routingConfiguration);
}
