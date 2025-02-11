using System.Text.Json.Serialization;

namespace radix_cap.services.Models.CoinGeckoModels;

public class Asset
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("current_price")]
    public decimal CurrentPrice { get; set; }

    [JsonPropertyName("market_cap")]
    public long MarketCap { get; set; }

    [JsonPropertyName("market_cap_rank")]
    public int MarketCapRank { get; set; }

    [JsonPropertyName("fully_diluted_valuation")]
    public long FullyDilutedValuation { get; set; }

    [JsonPropertyName("total_volume")]
    public double TotalVolume { get; set; }

    [JsonPropertyName("high_24h")]
    public double? High24H { get; set; }

    [JsonPropertyName("low_24h")]
    public double? Low24H { get; set; }

    [JsonPropertyName("price_change_24h")]
    public double? PriceChange24H { get; set; }

    [JsonPropertyName("price_change_percentage_24h")]
    public double? PriceChangePercentage24H { get; set; }

    [JsonPropertyName("market_cap_change_24h")]
    public double? MarketCapChange24H { get; set; }

    [JsonPropertyName("market_cap_change_percentage_24h")]
    public double? MarketCapChangePercentage24H { get; set; }

    [JsonPropertyName("circulating_supply")]
    public double CirculatingSupply { get; set; }

    [JsonPropertyName("total_supply")]
    public double TotalSupply { get; set; }

    [JsonPropertyName("max_supply")]
    public double? MaxSupply { get; set; }

    [JsonPropertyName("ath")]
    public double Ath { get; set; }

    [JsonPropertyName("ath_change_percentage")]
    public double AthChangePercentage { get; set; }

    [JsonPropertyName("ath_date")]
    public DateTime AthDate { get; set; }

    [JsonPropertyName("atl")]
    public double Atl { get; set; }

    [JsonPropertyName("atl_change_percentage")]
    public double AtlChangePercentage { get; set; }

    [JsonPropertyName("atl_date")]
    public DateTime AtlDate { get; set; }

    [JsonPropertyName("roi")]
    public Roi? Roi { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }
    
    [JsonPropertyName("sparkline_in_7d")]
    public SparklineSevenDays? SparklineIn7d { get; set; }

    public Radix_cap.data.models.Asset IntoDatabaseAsset()
    {
        return new Radix_cap.data.models.Asset
        {
            Id = Id,
            Symbol = Symbol,
            Name = Name,
            Image = Image,
            CurrentPrice = CurrentPrice,
            MarketCap = MarketCap,
            MarketCapRank = MarketCapRank,
            FullyDilutedValuation = FullyDilutedValuation,
            TotalVolume = TotalVolume,
            High24H = High24H,
            Low24H = Low24H,
            PriceChange24H = PriceChange24H,
            PriceChangePercentage24H = PriceChangePercentage24H,
            MarketCapChange24H = MarketCapChange24H,
            MarketCapChangePercentage24H = MarketCapChangePercentage24H,
            CirculatingSupply = CirculatingSupply,
            TotalSupply = TotalSupply,
            MaxSupply = MaxSupply,
            Ath = Ath,
            AthChangePercentage = AthChangePercentage,
            AthDate = AthDate,
            Atl = Atl,
            AtlChangePercentage = AtlChangePercentage,
            AtlDate = AtlDate,
            LastUpdated = LastUpdated
        };
    }
}