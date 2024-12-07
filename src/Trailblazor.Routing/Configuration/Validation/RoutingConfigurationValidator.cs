using Trailblazor.Routing.Exceptions;

namespace Trailblazor.Routing.Configuration.Validation;

internal sealed class RoutingConfigurationValidator : IRoutingConfigurationValidator
{
    public void ValidateAndThrowIfInvalid(IRoutingConfiguration routingConfiguration)
    {
        ValidateHierarchyNodesAreFlattened(routingConfiguration.NodesInHierarchy, new HashSet<INode>(routingConfiguration.FlattenedNodes));
        ValidateFlattenedNodesAreUnique(routingConfiguration);
        ValidateAllKeysAreUnique(routingConfiguration);
        ValidateAllUrisAreUnique(routingConfiguration);
        ValidateAllNodesWithUrisHaveAComponentType(routingConfiguration);
        ValidateAllComponentTypesAreTypesOfComponents(routingConfiguration);
        ValidateAllRelationshipsAreCorrectlySetUp(routingConfiguration);
        ValidateThereAreNoCircularReferences(routingConfiguration);
        ValidateRedirectOnNotFoundUri(routingConfiguration);
        ValidateNotFoundComponentType(routingConfiguration);
    }

    private void ValidateHierarchyNodesAreFlattened(IReadOnlyList<INode> nodesInHierarchy, HashSet<INode> flattenedNodes)
    {
        foreach (var node in nodesInHierarchy)
        {
            if (!flattenedNodes.Contains(node))
                throw new RoutingValidationException($"Node with key '{node.Key}' in the hierarchy is missing from the flattened list.");

            ValidateHierarchyNodesAreFlattened(node.Nodes, flattenedNodes);
        }
    }

    private void ValidateFlattenedNodesAreUnique(IRoutingConfiguration routingConfiguration)
    {
        if (routingConfiguration.FlattenedNodes.Distinct().Count() != routingConfiguration.FlattenedNodes.Count)
            throw new RoutingValidationException("FlattenedNodes contains duplicate node references.");
    }

    private void ValidateAllKeysAreUnique(IRoutingConfiguration routingConfiguration)
    {
        var allKeys = new HashSet<string>();
        foreach (var node in routingConfiguration.FlattenedNodes)
        {
            if (!allKeys.Add(node.Key))
                throw new RoutingValidationException($"Duplicate node key found: '{node.Key}'.");
        }
    }

    private void ValidateAllUrisAreUnique(IRoutingConfiguration routingConfiguration)
    {
        var allUris = new HashSet<string>();
        foreach (var node in routingConfiguration.FlattenedNodes)
        {
            foreach (var uri in node.Uris)
            {
                if (!allUris.Add(uri))
                    throw new RoutingValidationException($"Duplicate URI found: '{uri}' in node with key '{node.Key}'.");
            }
        }
    }

    private void ValidateAllNodesWithUrisHaveAComponentType(IRoutingConfiguration routingConfiguration)
    {
        foreach (var node in routingConfiguration.FlattenedNodes)
        {
            if (node.Uris.Count > 0 && node.ComponentType == null)
                throw new RoutingValidationException($"Node with key '{node.Key}' has at least one URI and therefore has to have a {nameof(INode.ComponentType)} defined.");
        }
    }

    private void ValidateAllComponentTypesAreTypesOfComponents(IRoutingConfiguration routingConfiguration)
    {
        foreach (var node in routingConfiguration.FlattenedNodes)
        {
            if (node.ComponentType != null)
                RoutingValidationException.ThrowIfTypeIsNotAComponent(node.ComponentType);
        }
    }

    private void ValidateAllRelationshipsAreCorrectlySetUp(IRoutingConfiguration routingConfiguration)
    {
        foreach (var node in routingConfiguration.FlattenedNodes)
            ValidateNodeRelationships(node);
    }

    private void ValidateNodeRelationships(INode node)
    {
        foreach (var child in node.Nodes)
        {
            if (child.ParentNode != node || (child.ParentNode != null && child.ParentNode.Key != node.Key))
                throw new RoutingValidationException($"Node with key '{child.Key}' does not have the correct parent set (expected '{node.Key}').");

            ValidateNodeRelationships(child);
        }
    }

    private void ValidateThereAreNoCircularReferences(IRoutingConfiguration routingConfiguration)
    {
        foreach (var node in routingConfiguration.FlattenedNodes)
            ValidateNoCircularReferences(node, []);
    }

    private void ValidateNoCircularReferences(INode node, HashSet<INode> visitedNodes)
    {
        if (visitedNodes.Contains(node))
            throw new RoutingValidationException($"Circular reference detected at node with key '{node.Key}'.");

        visitedNodes.Add(node);
        foreach (var child in node.Nodes)
            ValidateNoCircularReferences(child, visitedNodes);

        visitedNodes.Remove(node);
    }

    private void ValidateRedirectOnNotFoundUri(IRoutingConfiguration routingConfiguration)
    {
        if (!string.IsNullOrEmpty(routingConfiguration.NotFoundRedirectUri) && !Uri.IsWellFormedUriString(routingConfiguration.NotFoundRedirectUri, UriKind.RelativeOrAbsolute))
            throw new RoutingValidationException($"Invalid NotFoundRedirectUri: {routingConfiguration.NotFoundRedirectUri}");
    }

    private void ValidateNotFoundComponentType(IRoutingConfiguration routingConfiguration)
    {
        if (routingConfiguration.NotFoundComponentType != null)
            RoutingValidationException.ThrowIfTypeIsNotAComponent(routingConfiguration.NotFoundComponentType);
    }
}
