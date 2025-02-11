using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
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

    public async Task<PricePoint> GetCurrentPriceForAsset(string coinTicker)
    {
        return await _dbContext.Prices.Where((price) => price.AssetId == coinTicker).FirstAsync();
    }
}