using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Logging;
using Radix_cap.data.models;

namespace Radix_cap.data;

public class RadixCapDbContext(DbContextOptions<RadixCapDbContext> options) : DbContext(options)
{
    const string SERVER = "SPEEDY\\SQLEXPRESS";
    const string DATABASE = "radix_cap";
    const bool trustedConnection = true;
    const bool trustedServerCerificate = true;
    
    public DbSet<Asset> Assets { get; set; }
    public DbSet<PricePoint> Prices { get; set; }

    public DbSet<Roi> Rois { get; set; }
    
    public DbSet<Sparkline> Sparklines { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(SqlServerConnection())
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(LoggerFactory.Create(builder => 
                builder
                    .SetMinimumLevel(LogLevel.Warning)
                ));
    }
    
    
    public static string SqlServerConnection()
    {
        return
            $"Server={SERVER};Database={DATABASE};Trusted_Connection={trustedConnection};TrustServerCertificate={trustedServerCerificate};";
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        

        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                Console.WriteLine($"Entity Type: {entity.GetType().Name}");
        
                if (entity is Asset asset)
                {
                    void PrintDecimalDetails(string name, decimal? value)
                    {
                        if (value.HasValue)
                        {
                            var bits = decimal.GetBits(value.Value);
                            Console.WriteLine($"{name}: Value={value.Value}, Bits=[{string.Join(", ", bits)}]");
                        }
                        else
                        {
                            Console.WriteLine($"{name}: null");
                        }
                    }

                    PrintDecimalDetails("CurrentPrice", asset.CurrentPrice);
                    PrintDecimalDetails("MarketCap", asset.MarketCap);
                    PrintDecimalDetails("FullyDilutedValuation", asset.FullyDilutedValuation);
                    PrintDecimalDetails("TotalVolume", asset.TotalVolume);
                    PrintDecimalDetails("MarketCapChange24H", asset.MarketCapChange24H);
                    PrintDecimalDetails("CirculatingSupply", asset.CirculatingSupply);
                    PrintDecimalDetails("TotalSupply", asset.TotalSupply);
                    Type type = asset.GetType();
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        object value = prop.GetValue(asset, null);
                        Console.WriteLine($"{prop.Name}: {value}");
                    }
                }
            } 
            throw;
        }
    }
   
}