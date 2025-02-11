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
            var roi = asset.Roi?.IntoDatabaseRoi(asset.Name) ?? null;
            if (roi != null)
            {
                var existing = await context.Rois.FirstOrDefaultAsync(r => r.AssetId == roi.AssetId);

                if (existing == null)
                {
                    context.Rois.Add(roi);
                }
                else
                {
                    context.Entry(existing).CurrentValues.SetValues(roi);
                }
            }

            var dbAsset = asset.IntoDatabaseAsset();
            var existingDbAsset = await context.Assets.FirstOrDefaultAsync(a => a.Id == asset.Id);

            if (existingDbAsset == null)
            {
                context.Assets.Add(dbAsset);
            }
            else
            {
                context.Entry(existingDbAsset).CurrentValues.SetValues(dbAsset);
            }
        }
        await context.SaveChangesAsync();
    }
}