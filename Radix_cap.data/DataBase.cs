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
        var sevenDaysAgo = DateTime.Now.AddDays(-7).AddTicks(-1);
        var assets = _dbContext.Assets;
        var prices = _dbContext.Prices;
        Console.WriteLine($"StartRank: {startRank}, EndRank: {endRank}");

        var query = assets
            .Where(a => a.MarketCapRank >= startRank && a.MarketCapRank <= endRank)
            .Select(asset => new {
                Asset = asset,
                Prices = prices
                    .Where(p => p.AssetId == asset.Symbol && p.TimeStamp > sevenDaysAgo)
                    .ToArray()
            })
            .ToList();  

        return query
            .Select(result => AssetWithSparkline7D.FromAssetsAndPricePoints(
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