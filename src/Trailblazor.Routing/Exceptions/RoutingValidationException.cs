using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Trailblazor.Routing.Configuration.Validation;

/// <summary>
/// Exception is being thrown when routing validation errors arise.
/// </summary>
public sealed class RoutingValidationException : Exception
{
    internal RoutingValidationException(string message) : base(message)
    {
    }

    internal static void ThrowIfTypeIsNotAComponent(Type componentType, string? message = null)
    {
        if (componentType.IsAbstract || !componentType.IsAssignableTo(typeof(ComponentBase)))
        {
            message ??= $"Type '{componentType.FullName}' is not a non-abstract implementation of '{typeof(ComponentBase)}'";
            throw new RoutingValidationException(message);
        }
    }

    internal static void ThrowIfTypeIsNotAProfile(Type profileType)
    {
        if (profileType.IsAbstract || profileType.IsValueType || profileType.IsInterface || !profileType.IsAssignableTo(typeof(IRoutingProfile)))
            throw new RoutingValidationException($"The specified type '{profileType.FullName}' is not non-abstract implementation of the '{nameof(IRoutingProfile)}' interface.");
    }

    internal static void ThrowIfNodeWasNotFound([NotNull] INode? node, string key, string actionName)
    {
        if (node == null)
            throw new RoutingValidationException($"Node with key '{key}' could not be found in action '{actionName}'.");
    }

    internal static void ThrowIfUriAlreadyExistsForRouteNode(INode routeNode, string uri)
    {
        if (routeNode.HasUri(uri))
            throw new RoutingValidationException($"Node with key '{routeNode.Key}' already has an URI '{uri}'.");
    }
}
