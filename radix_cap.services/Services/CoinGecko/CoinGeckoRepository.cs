using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Radix_cap.data;
using radix_cap.services.Models.CoinGeckoModels;

namespace radix_cap.services.Services;

public class CoinGeckoRepository(RadixCapDbContext context) : ICoinGeckoRepository
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

            var pricePoints= asset.SparklineIn7d?.IntoDatabasePricePoints(asset.Symbol);
            if (pricePoints != null)
            {
                foreach (var pricePoint in pricePoints)
                {
                    var existingPricePoint = await context.Prices
                        .FirstOrDefaultAsync(
                            p => p.AssetId == pricePoint.AssetId && p.TimeStamp == pricePoint.TimeStamp);

                    if (existingPricePoint == null)
                    {
                        
                        context.Prices.Add(pricePoint);
                    }
                }
            }
            else
            {
                Console.WriteLine("No sparkline found");
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
                Console.WriteLine($"Failed to save {dbAsset.Symbol}:");
                // Console.WriteLine(string.Join("\n", values));
                // Console.WriteLine(ex.InnerException);
            }
        }
    }
}