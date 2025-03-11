//namespace Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed; // Add this using directive
using Scalar.AspNetCore;
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

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


string GenerateRandomString(int length)
{
    const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
    const string numbers = "0123456789";
    const string allCharacters = upperCase + lowerCase + numbers;

    Random random = new Random();
    return new string(Enumerable.Range(0, length)
                                .Select(_ => allCharacters[random.Next(allCharacters.Length)])
                                .ToArray());
}

app.MapPost("/create", async (UrlShortenRequest request, IDistributedCache distributedCache) =>
{
    // In a real application, you would typically hash the URL here
    // or store it in a database. For simplicity, we'll just use a random hash.

    string shortenedUrl = request.Hash ?? GenerateRandomString(8); // Generate a random hash if not provided
    string shortenedUrlLink = $"https://short.ly/{shortenedUrl}";

        // Store product in Redis cache for 5 minutes
    var options = new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

    await distributedCache.SetStringAsync(shortenedUrl, request.Url, options); // Store product name for simplicity

    // Return the shortened URL response
    return Results.Ok(new { OriginalUrl = request.Url, ShortenedUrl = shortenedUrlLink });
});

app.MapGet("/{hash}", async (string hash, IDistributedCache distributedCache) =>
{
    var url = await distributedCache.GetStringAsync(hash);

    if (!string.IsNullOrEmpty(url))
    {
        // Redirect to the URL if found
        return Results.Redirect(url);
    }

    // Return a 404 Not Found if the hash is not found
    return Results.NotFound("URL not found for the given hash.");
});

app.Run();

record UrlShortenRequest(string Url, string? Hash = null);
