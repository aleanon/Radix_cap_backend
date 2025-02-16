using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Radix_cap.data;
using radix_cap.services.Models.CoinGeckoModels;

namespace radix_cap.services.Services;

public class CoinGeckoRepository(RadixCapDbContext context, ILogger<CoinApiRepository> logger) : ICoinGeckoRepository
{
    public async Task SaveAssets(IEnumerable<Asset> assets)
    {
        foreach (var asset in assets)
        {
            var roi = asset.Roi?.IntoDatabaseRoi(asset.Name);
            if (roi != null)
            {
                var existing = await context.Rois.FirstOrDefaultAsync(r => r.AssetId == roi.AssetId);
            
                if (existing == null)
                {
                    context.Rois.Add(roi);
                }
                else
                {
                    existing.Currency = roi.Currency;
                    existing.Percentage = roi.Percentage;
                    existing.Times = roi.Times;
                }
            }

            var sparkline= asset.SparklineIn7d?.IntoSparkline(asset.Id);
            if (sparkline != null)
            {
                var existingSparkline = await context.Sparklines
                    .FirstOrDefaultAsync(
                        s => s.AssetId == asset.Id);

                if (existingSparkline == null)
                {
                    
                    context.Sparklines.Add(sparkline);
                }
            }
            else
            {
                logger.LogWarning("No sparkline found");
            }

            var dbAsset = asset.IntoDatabaseAsset();
            var existingDbAsset = await context.Assets.FirstOrDefaultAsync(a => a.Id == asset.Id && a.Symbol == asset.Symbol);

            if (existingDbAsset == null)
            {
                context.Add(dbAsset);
            }
            else
            {
                var propertiesToExclude = new[] { "Id", "Symbol" };
                var properties = typeof(Radix_cap.data.models.Asset).GetProperties()
                    .Where(p => !propertiesToExclude.Contains(p.Name));

                foreach (var prop in properties)
                {
                    prop.SetValue(existingDbAsset, prop.GetValue(dbAsset));
                }
            }
            
            try 
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // var values = dbAsset.GetType().GetProperties()
                //     .Where(p => p.PropertyType == typeof(decimal?))
                //     .Select(p => $"{p.Name}: {p.GetValue(dbAsset)}");
                logger.LogError( ex, "Failed to save {Symbol}", asset.Symbol);
                Type type = dbAsset.GetType();
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    object value = prop.GetValue(dbAsset, null);
                    Console.WriteLine($"{prop.Name}: {value}");
                }
                // Console.WriteLine(string.Join("\n", values));
                // Console.WriteLine(ex.InnerException);
            }
        }
    }
}