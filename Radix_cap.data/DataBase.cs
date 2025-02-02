using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Radix_cap.data.models;

namespace Radix_cap.data;

public class DataBase 
{
    
    private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;

    public DataBase()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(ApplicationDbContext.SqlServerConnection());
        _dbContextOptionsBuilder = optionsBuilder;
    }
    

    public async Task<List<Coin>> GetCoins()
    {
        await using var context = new ApplicationDbContext(_dbContextOptionsBuilder.Options);
        var coins = await context.Coins.ToListAsync();
        return coins;
    }

    public async Task<Price> GetCurrentPriceForAsset(string coin)
    {
        await using var context = new ApplicationDbContext(_dbContextOptionsBuilder.Options);
        var prices = await context.Prices.Where((price) => price.asset_name == coin).FirstAsync();
        return prices;
    }
}