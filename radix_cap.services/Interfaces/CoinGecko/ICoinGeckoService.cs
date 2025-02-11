using radix_cap.services.Models.CoinGeckoModels;

namespace radix_cap.services.Services;

public interface ICoinGeckoService
{
    Task<IEnumerable<Asset>> GetAssets(int page, int pageSize, bool withSparklines = false);
}