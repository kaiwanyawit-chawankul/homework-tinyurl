using Projects;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var webApi = builder.AddProject<homework_tinyurl_WebApi>("webApi")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithHttpEndpoint(name: "api");

builder.AddNpmApp("reactvite", "../homework-tinyurl.Vite")
    .WithReference(webApi)
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment("VITE_API_URL", webApi.GetEndpoint("api"))
    .WithHttpEndpoint(env: "VITE_PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

var app = builder.Build();

// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error", createScopeForErrors: true);
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();

// app.UseAntiforgery();

// app.UseOutputCache();

// app.MapStaticAssets();

// app.MapRazorComponents<App>()
//     .AddInteractiveServerRenderMode();

// app.MapDefaultEndpoints();

app.Run();
