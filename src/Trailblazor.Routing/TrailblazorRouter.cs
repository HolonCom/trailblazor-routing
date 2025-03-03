﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Trailblazor.Routing.Configuration;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

/// <summary>
/// Trailblazor router reacts to navigations and initiates the render of the registered <see cref="INode"/> that is associated with the current relative URI.
/// </summary>
/// <remarks>
/// <para>
///     How to use the router:<br/>
///     The router provides the render fragments <see cref="Found"/> and <see cref="NotFound"/>, pretty much like the standard Microsoft <see cref="Router"/>. Generated
///     <see cref="RouteData"/> is being exposed through the <see cref="Found"/> render fragment, so components such as the <see cref="RouteView"/> and <see cref="AuthorizeRouteView"/>
///     can use the <see cref="RouteData"/> to finally render the target components.
/// </para>
/// <para>
///     Using the <see cref="RouterContext"/>:<br/>
///     The <see cref="TrailblazorRouter"/> passes down a <see cref="RouterContext"/> as a cascading value. This way all components being renders as some child of the router can
///     safely accept a <see cref="RouterContext"/> as using the <see cref="CascadingParameterAttribute"/>. This cascading parameter is <b>never</b> <see langword="null"/>.<br/>
///     Alternatively you can inject the <see cref="IRouterContextProvider"/> into any service of component and access the current routing information through that service.
/// </para>
/// </remarks>
public sealed class TrailblazorRouter : IComponent, IHandleAfterRender, IDisposable
{
    private bool _navigationInterceptionEnabled;
    private RenderHandle _renderHandle;
    private string? _location;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private INavigationInterception NavigationInterception { get; set; } = null!;

    [Inject]
    private IRouterContextProvider RouterContextProvider { get; set; } = null!;

    /// <summary>
    /// Render fragment for content when a route has been found.
    /// </summary>
    [Parameter]
    public RenderFragment<RouteData>? Found { get; set; }

    /// <summary>
    /// Render fragment for content when a route has not been found.
    /// </summary>
    /// <remarks>
    /// You can configure the contents if not route was found for the current URI by configuring either the
    /// <see cref="IRoutingConfiguration.NotFoundComponentType"/> or <see cref="IRoutingConfiguration.NotFoundRedirectUri"/> instead.
    /// </remarks>
    [Parameter]
    public RenderFragment? NotFound { get; set; }

    /// <inheritdoc/>
    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    /// <inheritdoc/>
    public void Attach(RenderHandle renderHandle)
    {
        _renderHandle = renderHandle;
        _location = NavigationManager.GetRelativeUri();

        NavigationManager.LocationChanged += OnLocationChanged;
    }

    /// <inheritdoc/>
    public async Task OnAfterRenderAsync()
    {
        if (!_navigationInterceptionEnabled)
        {
            _navigationInterceptionEnabled = true;
            await NavigationInterception.EnableNavigationInterceptionAsync();
        }
    }

    /// <inheritdoc/>
    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        InitiateRender();
        return Task.CompletedTask;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        _location = args.Location;
        InitiateRender();
    }

    private void InitiateRender()
    {
        if (_location == null)
            return;

        RouterContextProvider.UpdateRouterContext();
        var routerContext = RouterContextProvider.GetRouterContext();

        _renderHandle.Render(builder =>
        {
            builder.OpenComponent<CascadingValue<RouterContext>>(0);
            builder.AddComponentParameter(1, nameof(CascadingValue<RouterContext>.Value), routerContext);
            builder.AddComponentParameter(2, nameof(CascadingValue<RouterContext>.ChildContent), (RenderFragment)(content =>
            {
                var renderFragment = routerContext.RouteData != null && Found != null
                    ? Found.Invoke(routerContext.RouteData)
                    : NotFound;

                content.AddContent(2, renderFragment);
            }));
            builder.CloseComponent();
        });
    }
}
