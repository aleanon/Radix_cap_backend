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
}