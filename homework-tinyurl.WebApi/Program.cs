//namespace Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed; // Add this using directive

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Redis caching
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = "localhost:53511"; // Redis connection string
//     options.InstanceName = "SampleInstance"; // Optional
// });
builder.AddRedisDistributedCache("cache");
//builder.Services.AddRedis();
//builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/product/{id}", async (int id, IDistributedCache distributedCache) =>
{
    string cacheKey = $"Product_{id}";
    var cachedProduct = await distributedCache.GetStringAsync(cacheKey);

    if (string.IsNullOrEmpty(cachedProduct))
    {
        // Simulate fetching from DB
        var product = new Product(id, "Sample Product");

        // Store product in Redis cache for 5 minutes
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        await distributedCache.SetStringAsync(cacheKey, product.Name, options); // Store product name for simplicity

        return Results.Ok(product);
    }

    return Results.Ok(cachedProduct);
});

app.Run();

record Product(int Id, string Name);
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
