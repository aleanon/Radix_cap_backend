using System.Text.Json;
using System.Text.Json.Serialization;
using Radix_cap.data.models;

namespace radix_cap.services.Models.CoinGeckoModels;

public class SparklineSevenDays
{
    [JsonPropertyName("price")]
    public decimal[] Price { get; set; }

    public PricePoint[] IntoDatabasePricePoints(string assetSymbol)
    {
        var now = DateTime.UtcNow;
        var result = Price.Reverse().Select((p, index) =>
        {
            var timestamp = now.AddHours(-index);
            return new PricePoint
            {
                Price = p,
                TimeStamp = timestamp,
                AssetId = assetSymbol,
            };
        }).ToArray();
        return result;
    }

    public Sparkline IntoSparkline(string assetId)
    {
        return new Sparkline
        {
            AssetId = assetId,
            Prices = JsonSerializer.Serialize(Price),
        };
    }
}