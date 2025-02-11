using System.Text.Json.Serialization;
using Radix_cap.data.models;

namespace radix_cap.services.Models.CoinApiModels;

public class TimeSerie
{
    [JsonPropertyName("time_period_start")]
    public DateTime TimePeriodStart { get; set; }

    [JsonPropertyName("time_period_end")]
    public DateTime TimePeriodEnd { get; set; }

    [JsonPropertyName("time_open")]
    public DateTime TimeOpen { get; set; }

    [JsonPropertyName("time_close")]
    public DateTime TimeClose { get; set; }

    [JsonPropertyName("rate_open")]
    public double RateOpen { get; set; }

    [JsonPropertyName("rate_high")]
    public double RateHigh { get; set; }

    [JsonPropertyName("rate_low")]
    public double RateLow { get; set; }

    [JsonPropertyName("rate_close")]
    public decimal RateClose { get; set; }

    public PricePoint IntoPricePoint(string assetId)
    {
        return new PricePoint
        {
            AssetId = assetId,
            TimeStamp = TimeClose,
            Price = RateClose,
        };
    }
}