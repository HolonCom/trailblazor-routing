using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.App.WASM;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddTrailblazorRouting(options =>
        {
            options.AddProfilesFromAssemblies(Assembly.Load("Trailblazor.Routing.App.WASM"));
        });

        await builder.Build().RunAsync();
    }
}
