﻿namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that a given type is an abstract profile type and can therefore not be registered.
/// </summary>
public sealed class AbstractProfileRegisteredException : Exception
{
    internal AbstractProfileRegisteredException(Type abstractProfileType)
        : base($"An abstract type of a profile has been attempted to be registered: {abstractProfileType.FullName}")
    {
    }
}
