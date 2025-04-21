using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HostingTest;
using HostingTest.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddOptions<List<Document>>().Bind(builder.Configuration.GetSection("Document"));
builder.Services.AddOptions<List<Person>>().Bind(builder.Configuration.GetSection("People"));

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();