using radix_cap.services.Models.CoinGeckoModels;

namespace radix_cap.services.Services;

public interface ICoinGeckoRepository
{
    Task SaveAssets(IEnumerable<Asset> assets);
}