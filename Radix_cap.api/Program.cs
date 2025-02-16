using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Radix_cap.data;
using radix_cap.services.Services;

var builder = WebApplication.CreateBuilder(args);
var defaultCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;


builder.Services.AddDbContext<RadixCapDbContext>();
// options => options.UseSqlServer(RadixCapDbContext.SqlServerConnection()));

builder.Services.AddScoped<DataBase>();
builder.Services.AddHttpClient<ICoinGeckoService, CoinGeckoService>();
builder.Services.AddScoped<ICoinGeckoService, CoinGeckoService>();
builder.Services.AddScoped<ICoinGeckoRepository, CoinGeckoRepository>();
builder.Services.AddHostedService<CoinGeckoBackgroundService>();

// builder.Services.AddHttpClient<ICoinApiService, CoinApiService>();
// builder.Services.AddTransient<ICoinApiService, CoinApiService>();
// builder.Services.AddTransient<ICoinApiRepository, CoinApiRepository>();
// builder.Services.AddHostedService<CoinApiTimeSeriesBackgroundService>();
// builder.Services.AddHostedService<CoinApiCurrentPricesBackgroundService>();

builder.Services.AddOpenApi();
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);
    options.SizeLimit = 100 * 1024 * 1024;
});


builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/json" });
});




builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-origin-when-cross-origin");
    context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseResponseCaching();
app.UseOutputCache();
app.UseRateLimiter();
app.UseCors("ProductionPolicy");



app.MapGet("/current_price", async (string coinTicker, DataBase db) => await db.GetCurrentPriceForAsset(coinTicker))
    .WithName("CurrentPrice");

app.MapGet("/Coins/", async (HttpContext httpContext, DataBase db, int page = 1, int assetsPrPage=100) =>
{
    var startRank = ((page - 1) * assetsPrPage) + 1;
    var endRank = startRank + assetsPrPage - 1;
    var coins = await db.GetAssetsWithSparklineRankRange(startRank, endRank);
    Console.WriteLine($"{coins.Count}");
    httpContext.Response.Headers.ContentType = "application/json";
    httpContext.Response.StatusCode = 200;

    if (!httpContext.Request.Headers.AcceptEncoding.ToString().Contains("gzip"))
    {
        await JsonSerializer.SerializeAsync(httpContext.Response.Body, coins);
    }
    else
    {
        httpContext.Response.Headers.ContentEncoding = "gzip";

        await using var compressionStream = new GZipStream(httpContext.Response.Body, CompressionLevel.Optimal);
        
        await JsonSerializer.SerializeAsync(compressionStream, coins);
    }
});

app.Run();

