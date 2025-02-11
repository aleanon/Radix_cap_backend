using radix_cap.services.Models.CoinApiModels;

namespace radix_cap.services.Services;



public interface ICoinApiService
{
    Task<IEnumerable<Asset>> GetAllAssets();
    Task<IEnumerable<TimeSerie>> GetTimeSeriesForAsset(string assetSymbol, DateTime startTime, DateTime endTime, string period);
    Task<CurrentRates> GetCurrentRatesForAllAssets();
}