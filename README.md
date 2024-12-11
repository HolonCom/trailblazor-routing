# Trailblazor Routing
**Trailblazor Routing** is a flexible and alternative Approach to routing and navigation for Blazor applications.

By default routes are scanned for and registered through `@page` directives at the top of components. While thats still possible, **Trailblazor Routing** allows setting up `RoutingProfiles` to configure routes and their metadata.

## Blazor Support
There are different types of Blazor applications and not all of them are supported as of right now. Following types of Blazor apps are supported:
- Blazor WASM
- MAUI Blazor

### Why no Blazor Server Support?
Since Blazor Servers are just regular ASP.NET Core web apps juiced up with Blazor services and a Blazor SignalR hub, I assume I need to do more than just implement a custom router Razor component. I think the components URIs are also mapped as endpoints in some way. Again, as of right now this is not done by Trailblazor Routing and in order to be able to open up an HTTP request to some component route successfully the Razor components in question still need an `@page` directive.

I have looked into it very briefly and I assume I might even have to manually add the render modes to components or so, but I am not sure. I am also not sure that I can even do that from outside of the framework if I have to.

### Is Blazor Server Support going to be added?
I am going to look into it when I have time, but no promises are made.

## Setup
1. Add the `AddTrailblazorRouting(IRoutingOptionsBuilder)` extension method to your `IServiceCollection`.
2. Use the `IRoutingOptionsBuilder` to configure routing specifics and register `IRoutingProfile`.
3. Implement one or mutliple `IRoutingProfile` and use the exposed `IRoutingConfigurationBuilder` to configure the `IRoutingConfiguration`. This holds node registrations and other details.
4. Replace the microsoft `Router` component with the `TrailblazorRouter` component. The `Found` and `NotFound` renderfragments and their contents such as the `RouteView` or `AuthorizeRouteView` can stay. The content displayed when a route has not been found however can also be set using the `IRoutingConfigurationBuilder`.

### Startup DI
```cs
builder.Services.AddTrailblazorRouting(options =>
{
    var assembly = Assembly.Load("My.Example.App");

    options.AddProfile<MyRoutingProfile>();
    options.AddProfile(typeof(MyOtherProfile));

    options.AddProfilesFromAssemblies(assembly);        // Scan for IRoutingProfiles
    options.ScanForNodesInAssemblies(assembly);         // Scan for components with an @page directive and optional attributes configuring the node

    options.DisableRoutingConfigurationValidation();    // Disable validating the IRoutingConfiguration after it has been fully configured. Validation is on by default

    options.ConfigureConfiguration(builder =>
    {
        // Configure the IRoutingConfiguration in the extension method directly
        // This action is used after all routing profiles have been run through!
    });
});
```

### Routing Profile
Routing profiles are always registered as a transient `IRoutingProfile` service. They are resolved using the `IServiceProvider` the first time the `IRoutingConfiguration` is accessed. Therefore routing profiles enjoy full dependency injection support!

```cs
internal sealed class RoutingProfile(IConfiguration _configuration) : IRoutingProfile
{
    public void ConfigureRoutes(IRoutingConfigurationBuilder builder)
    {
        builder.AddNode<Home>("Home", "/", n => n.WithUris("/home", "/landing-page"));
        builder.AddNode("Content", g =>
        {
            g.AddNode<Counter>("Counter", "/counter", n => n.WithUris("/counter/{count}"));
            g.AddNode<Weather>("Weather", "/weather");
        });

        // Use the injected configuration or so...
    }
}
```

### Router
```html
<TrailblazorRouter>
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />

        <FocusOnNavigate RouteData="@routeData" Selector="h1" />
    </Found>
</TrailblazorRouter>
```

### Registering Components as Nodes using attributes
Components can be scanned for through their assemblies as shown above. In order to be picked up they need either a `@page` directive or a `RouteAttribute`. Trailblazor specifics can also be configured using attributes. There are a few self-explainatory ones, all of which have sufficient documentation as comments.
- `NodeKeyAttribute`: Sets the nodes key. Optional, but if not set then the name of the component will be used as a key and since keys have to be unique this could potentially cause conflicts down the road
- `NodeParentAttribute`: Sets the key of the nodes parent node. Optional
- `NodeMetadataAttribute`: Sets metadata of the node. Optional, multiple attributes are allowed

### `IRoutingOptions` vs `IRoutingConfiguration`
The `IRoutingOptions` contain settings about the orchestration of the framework. Logging, validation or what type of profiles are used are more administrative than functional. The `IRoutingConfiguration` contains direct registrations of nodes and "routes", so simply nodes with URIs and a component type.

Both are accessible through their respective provider services, the `IRoutingOptionsProvider` and `IRoutingConfigurationProvider`.

## Route Parameters
There is **full support** for default parameters in the URI like in vanilla Blazor routing. Additionally standard URL query parameters are supported as well. Both types of parameters can be used in tandem or individually.

### Configuring Route Parameters
#### 1. The usual way
The component has an `@page` directive with its parameter configured directly in its URI.

```html
@page "/counter/{Count:int}"

@attribute [NodeKey("counter")]
@attribute [NodeMetadata("metadata-key", "metadata-value")]

<h1>Counter</h1>

<p role="status">Current count: @Count</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    [Parameter]
    public int Count { get; set; }

    private void IncrementCount() => Count++;
}
```

#### 2. The new way
The 'count' parameter can now be addressed as an inline parameter in the URL, but also as a query parameter. The `QueryParameterAttribute` flags a property to be a query parameter. Thus it is required to place a `ParameterAttribute` above it. A static name of the query parameter key can bet set using the `QueryParameterAttribute` attribute. This will also be the name of the parameter in the inline URI when registering it.

```html
<h1>Counter</h1>

<p role="status">Current count: @Count</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@if (FlavorText != null)
{
    <p>Flavor text: @FlavorText</p>
}

@code {
    [Parameter]
    [QueryParameter("count")]
    public int Count { get; set; }

    [Parameter]
    [QueryParameter("flavor")]
    public string? FlavorText { get; set; }

    private void IncrementCount() => Count++;
}
```

The component added as a node in a routing profile. Note that the 'count' parameter is configure inline, while the 'flavor' is not. Both are still addressable as query parameters. Its your choice how you want to use this.
```cs
builder.AddNode<Counter>("Counter", "/counter", n => n.WithUris("/counter/{count}"));
```

## Accessing Routing Details
Accessing the currently navigated to `INode` or parameters from the URI is very simple. The `TrailblazorRouter` always passes down an instance of the `RouterContext` as a cascading value. This cascading parameter will **never** be null.

```html
@page "/metadata"

@attribute [NodeKey("metadata")]
@attribute [NodeParent("metadata")]
@attribute [NodeMetadata("cool", "metadata")]

@{
    var metadatas = GetMetadata();
}

@if (metadatas.Length > 0)
{
    <div>
        @foreach (var metadata in metadatas)
        {
            <p>@metadata</p>
        }
    </div>
}

@code {
    [CascadingParameter]
    private RouterContext Context { get; set; } = null!;

    private string[] GetMetadata()
    {
        if (Context.RouteNode == null)
            return [];

        return Context.RouteNode.Metadata.Select(m => $"{m.Key}; {m.Value}").ToArray();
    }
}
```

Additionally the `IRouterContextAccessor` service provides the same instance of the current `RouterContext`. Inject the service to access routing data in your own services. You can also subscribe to it to catch when the `RouterContext` has been updated.

## Accessing or resolving configured Nodes
Either the `IRoutingConfigurationProvider` can be used to provide the `IRoutingConfiguration`, which contains all registered nodes as both a hierarchical list, for when you want to implement breadcrumbsn or a menu, and a flattened list of nodes that simply contains every node.

However you can also use the `INodeProvider` service to make some operations a little more easy.

The `INodeResolver` service is used internally to parse parameters from a relative URI and find the `INode` that matches the URI.

## Handling a not-found route
As mentioned above the `NotFound` render fragment of the router component can be used. Additionally the `IRoutingConfiguration` contains information about what happens when no node for the current relative URI was found.

You can either set a redirect URI, that is to be redirected when no node could be resolved, or you can set a component that is to be rendered instead of the, what would be the target nodes component. Both can be set at the same time but the redirect URI will always be prioritized.
