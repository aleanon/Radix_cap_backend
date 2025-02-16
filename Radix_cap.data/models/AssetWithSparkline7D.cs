using System.Text.Json.Serialization;
using Radix_cap.data.models;

namespace Radix_cap.api.Models;


public class AssetWithSparkline7D
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

    
    public static AssetWithSparkline7D FromAssetsAndPricePoints(Asset asset, PricePoint[] pricePoints)
    {
        return new AssetWithSparkline7D
        {
            Id = asset.Id,
            Symbol = asset.Symbol,
            Name = asset.Name,
            Image = asset.Image,
            CurrentPrice = asset.CurrentPrice,
            MarketCap = asset.MarketCap,
            MarketCapRank = asset.MarketCapRank,
            FullyDilutedValuation = asset.FullyDilutedValuation,
            TotalVolume = asset.TotalVolume,
            High24H = asset.High24H,
            Low24H = asset.Low24H,
            PriceChange24H = asset.PriceChange24H,
            PriceChangePercentage24H = asset.PriceChangePercentage24H,
            MarketCapChange24H = asset.MarketCapChange24H,
            MarketCapChangePercentage24H = asset.MarketCapChangePercentage24H,
            CirculatingSupply = asset.CirculatingSupply,
            TotalSupply = asset.TotalSupply,
            MaxSupply = asset.MaxSupply,
            Ath = asset.Ath,
            AthChangePercentage = asset.AthChangePercentage,
            AthDate = asset.AthDate,
            Atl = asset.Atl,
            AtlChangePercentage = asset.AtlChangePercentage,
            AtlDate = asset.AtlDate,
            LastUpdated = asset.LastUpdated,
            SparklineIn7d = SparklineSevenDays.FromPricePoints(pricePoints)
        };
    }
    
    public static AssetWithSparkline7D FromAssetsAndSparkline(Asset asset, Sparkline? sparkline)
    {
        return new AssetWithSparkline7D
        {
            Id = asset.Id,
            Symbol = asset.Symbol,
            Name = asset.Name,
            Image = asset.Image,
            CurrentPrice = asset.CurrentPrice,
            MarketCap = asset.MarketCap,
            MarketCapRank = asset.MarketCapRank,
            FullyDilutedValuation = asset.FullyDilutedValuation,
            TotalVolume = asset.TotalVolume,
            High24H = asset.High24H,
            Low24H = asset.Low24H,
            PriceChange24H = asset.PriceChange24H,
            PriceChangePercentage24H = asset.PriceChangePercentage24H,
            MarketCapChange24H = asset.MarketCapChange24H,
            MarketCapChangePercentage24H = asset.MarketCapChangePercentage24H,
            CirculatingSupply = asset.CirculatingSupply,
            TotalSupply = asset.TotalSupply,
            MaxSupply = asset.MaxSupply,
            Ath = asset.Ath,
            AthChangePercentage = asset.AthChangePercentage,
            AthDate = asset.AthDate,
            Atl = asset.Atl,
            AtlChangePercentage = asset.AtlChangePercentage,
            AtlDate = asset.AtlDate,
            LastUpdated = asset.LastUpdated,
            SparklineIn7d = SparklineSevenDays.FromDbSparkline(sparkline)
        };
    }
        
    
}