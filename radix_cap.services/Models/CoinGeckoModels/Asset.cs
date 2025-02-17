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
    public string? Image { get; set; }

    [JsonPropertyName("current_price")]
    public decimal? CurrentPrice { get; set; }

    [JsonPropertyName("market_cap")]
    public decimal? MarketCap { get; set; }

    [JsonPropertyName("market_cap_rank")]
    public int? MarketCapRank { get; set; }

    [JsonPropertyName("fully_diluted_valuation")]
    public decimal? FullyDilutedValuation { get; set; }

    [JsonPropertyName("total_volume")]
    public decimal? TotalVolume { get; set; }

    [JsonPropertyName("high_24h")]
    public decimal? High24H { get; set; }

    [JsonPropertyName("low_24h")]
    public decimal? Low24H { get; set; }

    [JsonPropertyName("price_change_24h")]
    public decimal? PriceChange24H { get; set; }

    [JsonPropertyName("price_change_percentage_24h")]
    public decimal? PriceChangePercentage24H { get; set; }

    [JsonPropertyName("market_cap_change_24h")]
    public decimal? MarketCapChange24H { get; set; }

    [JsonPropertyName("market_cap_change_percentage_24h")]
    public decimal? MarketCapChangePercentage24H { get; set; }

    [JsonPropertyName("circulating_supply")]
    public decimal? CirculatingSupply { get; set; }

    [JsonPropertyName("total_supply")]
    public decimal? TotalSupply { get; set; }

    [JsonPropertyName("max_supply")]
    public decimal? MaxSupply { get; set; }

    [JsonPropertyName("ath")]
    public decimal? Ath { get; set; }

    [JsonPropertyName("ath_change_percentage")]
    public decimal? AthChangePercentage { get; set; }

    [JsonPropertyName("ath_date")]
    public DateTime? AthDate { get; set; }

    [JsonPropertyName("atl")]
    public decimal? Atl { get; set; }

    [JsonPropertyName("atl_change_percentage")]
    public decimal? AtlChangePercentage { get; set; }

    [JsonPropertyName("atl_date")]
    public DateTime? AtlDate { get; set; }

    [JsonPropertyName("roi")]
    public Roi? Roi { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime? LastUpdated { get; set; }
    
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
        CurrentPrice = CurrentPrice.HasValue ? Math.Round((decimal)CurrentPrice, 18) : null,
        MarketCap = MarketCap.HasValue ? Math.Round((decimal)MarketCap, 0) : null,
        MarketCapRank = MarketCapRank,
        FullyDilutedValuation = FullyDilutedValuation.HasValue ? Math.Round((decimal)FullyDilutedValuation, 0) : null,
        TotalVolume = TotalVolume.HasValue ? Math.Round((decimal)TotalVolume, 8) : null,
        High24H = High24H.HasValue ? Math.Round((decimal)High24H, 18) : null,
        Low24H = Low24H.HasValue ? Math.Round((decimal)Low24H, 18) : null,
        PriceChange24H = PriceChange24H.HasValue ? Math.Round((decimal)PriceChange24H, 18) : null,
        PriceChangePercentage24H = PriceChangePercentage24H.HasValue ? Math.Round((decimal)PriceChangePercentage24H, 8) : null,
        MarketCapChange24H = MarketCapChange24H.HasValue ? Math.Round((decimal)MarketCapChange24H, 8) : null,
        MarketCapChangePercentage24H = MarketCapChangePercentage24H.HasValue ? Math.Round((decimal)MarketCapChangePercentage24H, 18) : null,
        CirculatingSupply = CirculatingSupply.HasValue ? Math.Round((decimal)CirculatingSupply, 0) : null,
        TotalSupply = TotalSupply.HasValue ? Math.Round((decimal)TotalSupply, 0) : null,
        MaxSupply = MaxSupply.HasValue ? Math.Round((decimal)MaxSupply, 0) : null,
        Ath = Ath.HasValue ? Math.Round((decimal)Ath, 18) : null,
        AthChangePercentage = AthChangePercentage.HasValue ? Math.Round((decimal)AthChangePercentage, 8) : null,
        AthDate = AthDate,
        Atl = Atl.HasValue ? Math.Round((decimal)Atl, 18) : null,
        AtlChangePercentage = AtlChangePercentage.HasValue ? Math.Round((decimal)AtlChangePercentage, 8) : null,
        AtlDate = AtlDate,
        LastUpdated = LastUpdated
    };
}
}