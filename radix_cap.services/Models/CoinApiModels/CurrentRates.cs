using System.Text.Json.Serialization;

namespace radix_cap.services.Models.CoinApiModels;

public class CurrentRates
{
    [JsonPropertyName("asset_id_base")]
    public string AssetIdBase { get; set; }
    [JsonPropertyName("rates")]
    public AssetRate[] AssetRates { get; set; }
}