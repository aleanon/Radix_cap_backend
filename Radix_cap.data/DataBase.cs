using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Radix_cap.api.Models;
using Radix_cap.data.models;

namespace Radix_cap.data;

public class DataBase 
{
    
    private readonly RadixCapDbContext _dbContext;

    public DataBase()
    {
        var optionsBuilder = new DbContextOptionsBuilder<RadixCapDbContext>();
        optionsBuilder.UseSqlServer(RadixCapDbContext.SqlServerConnection());
        _dbContext = new RadixCapDbContext(optionsBuilder.Options);
    }
    

    public async Task<List<Asset>> GetCoins()
    {
        return await _dbContext.Assets.ToListAsync();
    }

    public async Task<List<AssetWithSparkline7D>> GetAssetsWithSparklineRankRange(int startRank, int endRank)
    {
        var assets = _dbContext.Assets;
        var prices = _dbContext.Sparklines;
        Console.WriteLine($"StartRank: {startRank}, EndRank: {endRank}");

        var query = assets
            .Where(a => a.MarketCapRank >= startRank && a.MarketCapRank <= endRank)
            .Select(asset => new {
                Asset = asset,
                Prices = prices
                    .FirstOrDefault(p => p.AssetId == asset.Id)
            })
            .ToList();  

        return query
            .Select(result => AssetWithSparkline7D.FromAssetsAndSparkline(
                result.Asset, 
                result.Prices
            ))
            .ToList();
    }

    public async Task<PricePoint> GetCurrentPriceForAsset(string coinTicker)
    {
        return await _dbContext.Prices.Where((price) => price.AssetId == coinTicker).FirstAsync();
    }
}