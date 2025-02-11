using System.Text.Json.Serialization;

namespace radix_cap.services.Models.CoinGeckoModels;

public class Roi
{
    [JsonPropertyName("times")]
    public double Times { get; set; }
    [JsonPropertyName("currency")]
    public string Currency  { get; set; }
    [JsonPropertyName("percentage")]
    public double Percentage { get; set; }

    public Radix_cap.data.models.Roi IntoDatabaseRoi(string assetId)
    {
        return new Radix_cap.data.models.Roi
        {
            AssetId = assetId,
            Times = Times,
            Currency = Currency,
            Percentage = Percentage
        };
    }
}