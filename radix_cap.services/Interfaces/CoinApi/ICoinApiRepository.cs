using Radix_cap.data.models;
using radix_cap.services.Models.CoinApiModels;
using Asset = Radix_cap.data.models.Asset;

namespace radix_cap.services.Services;

public interface ICoinApiRepository
{
    Task SaveCurrentRates(CurrentRates currentRates);
    Task<IEnumerable<Asset>> GetAllAssets();
    Task<DateTime> GetTimeSeriesStartForAsset(Asset asset, DateTime defaultStartTime);

    Task SaveTimeSeries(IEnumerable<TimeSerie> timeSeries, string assetId);
}