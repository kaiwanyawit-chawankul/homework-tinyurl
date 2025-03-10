using Projects;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

builder.AddProject<homework_tinyurl_WebApi>("webApi")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache);

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
