using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Radix_cap.data;
using radix_cap.services.Models.CoinApiModels;
using Asset = Radix_cap.data.models.Asset;

namespace radix_cap.services.Services;

public class CoinApiRepository(RadixCapDbContext context) : ICoinApiRepository
{
    public async Task SaveCurrentRates(CurrentRates currentRates)
    {
        foreach (var assetRate in currentRates.AssetRates)
        {
            var currentPrice = Math.Round(assetRate.Rate, 18);
            var asset = await context.Assets.FirstOrDefaultAsync(a => a.Symbol == assetRate.AssetId);
            if (asset == null) continue;
            asset.CurrentPrice = currentPrice;
            asset.LastUpdated = assetRate.Time;
        }

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Asset>> GetAllAssets()
    {
        return await context.Assets.ToListAsync();
    }

    public async Task<DateTime> GetTimeSeriesStartForAsset(Asset asset, DateTime defaultStartTime)
    {
        var startTime = await context.Prices
            .Where(p => p.AssetId == asset.Symbol)
            .MaxAsync(p => (DateTime?)p.TimeStamp) ?? defaultStartTime;

        return startTime;
    }

    public async Task SaveTimeSeries(IEnumerable<TimeSerie> timeSeries, string assetId)
    {
        foreach (var timeSerie in timeSeries)
        {
            var pricePoint = timeSerie?.IntoPricePoint(assetId);
            if (pricePoint == null) continue;
            var exists = await context.Prices
                .FirstOrDefaultAsync(p => p.AssetId == pricePoint.AssetId && p.TimeStamp == pricePoint.TimeStamp);
            if (exists != null) continue;

            context.Prices.Add(pricePoint);
        }

        await context.SaveChangesAsync();
    }
}