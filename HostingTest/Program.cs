using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HostingTest;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var baseAddress = builder.HostEnvironment.IsProduction()
    ? "hostingTest/"
    : builder.HostEnvironment.BaseAddress;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(baseAddress) });

await builder.Build().RunAsync();