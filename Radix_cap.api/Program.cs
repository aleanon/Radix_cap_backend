using Microsoft.EntityFrameworkCore;
using Radix_cap.data;
using radix_cap.services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RadixCapDbContext>(options =>
    options.UseSqlServer(RadixCapDbContext.SqlServerConnection()));

builder.Services.AddScoped<DataBase>();
builder.Services.AddHttpClient<ICoinGeckoService, CoinGeckoService>();
builder.Services.AddScoped<ICoinGeckoService, CoinGeckoService>();
builder.Services.AddScoped<ICoinGeckoRepository, CoinGeckoRepository>();
builder.Services.AddHostedService<CoinGeckoBackgroundService>();

builder.Services.AddHttpClient<ICoinApiService, CoinApiService>();
builder.Services.AddScoped<ICoinApiService, CoinApiService>();
builder.Services.AddScoped<ICoinApiRepository, CoinApiRepository>();
builder.Services.AddHostedService<CoinApiBackgroundService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapGet("/coins", async (DataBase db) => await db.GetCoins())
    .WithName("Coins");

app.MapGet("/current_price", async (string coinTicker, DataBase db) => await db.GetCurrentPriceForAsset(coinTicker))
    .WithName("CurrentPrice");

app.Run();

