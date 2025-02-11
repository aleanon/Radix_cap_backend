using System.Text.Json.Serialization;

namespace radix_cap.services.Models.CoinGeckoModels;

public class SparklineSevenDays
{
    [JsonPropertyName("price")]
    public double[] Price { get; set; }
}