using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Radix_cap.data.models;

[Table("assets")]
[PrimaryKey(nameof(Id), nameof(Symbol) )]
public class Asset
{
    [MaxLength(100)]
    public string Id { get; set; }  

    [MaxLength(20)]
    public string Symbol { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? Image { get; set; }

    public decimal? CurrentPrice { get; set; }

    public decimal? MarketCap { get; set; }

    public int? MarketCapRank { get; set; }

    public decimal? FullyDilutedValuation { get; set; }

    public decimal? TotalVolume { get; set; }

    public decimal? High24H { get; set; }

    public decimal? Low24H { get; set; }

    public decimal? PriceChange24H { get; set; }

    public decimal? PriceChangePercentage24H { get; set; }

    public decimal? MarketCapChange24H { get; set; }

    public decimal? MarketCapChangePercentage24H { get; set; }

    public decimal? CirculatingSupply { get; set; }

    public decimal? TotalSupply { get; set; }

    public decimal? MaxSupply { get; set; }

    public decimal? Ath { get; set; }

    public decimal? AthChangePercentage { get; set; }

    public DateTime? AthDate { get; set; }

    public decimal? Atl { get; set; }

    public decimal? AtlChangePercentage { get; set; }

    public DateTime? AtlDate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public Asset(){}
    public Asset(string name, string symbol)
    {
        Id = name.ToLower();
        Symbol = symbol;
        name = name;
    }
    
    public void RoundDecimalsToMatchDb()
    {
        if (CurrentPrice.HasValue) CurrentPrice = decimal.Round(CurrentPrice.Value, 18);
        if (MarketCap.HasValue) MarketCap = decimal.Round(MarketCap.Value, 0);
        if (FullyDilutedValuation.HasValue) FullyDilutedValuation = (decimal)decimal.Round(FullyDilutedValuation.Value, 0);
        if (TotalVolume.HasValue) TotalVolume = decimal.Round(TotalVolume.Value, 8);
        if (High24H.HasValue) High24H = decimal.Round(High24H.Value, 18);
        if (Low24H.HasValue) Low24H = decimal.Round(Low24H.Value, 18);
        if (PriceChange24H.HasValue) PriceChange24H = decimal.Round(PriceChange24H.Value, 18);
        if (PriceChangePercentage24H.HasValue) PriceChangePercentage24H = decimal.Round(PriceChangePercentage24H.Value, 8);
        if (MarketCapChange24H.HasValue) MarketCapChange24H = decimal.Round(MarketCapChange24H.Value, 8);
        if (MarketCapChangePercentage24H.HasValue) MarketCapChangePercentage24H = decimal.Round(MarketCapChangePercentage24H.Value, 18);
        if (CirculatingSupply.HasValue) CirculatingSupply = decimal.Round(CirculatingSupply.Value, 0);
        if (TotalSupply.HasValue) TotalSupply = decimal.Round(TotalSupply.Value, 0);
        if (MaxSupply.HasValue) MaxSupply = decimal.Round(MaxSupply.Value, 0);
        if (Ath.HasValue) Ath = decimal.Round(Ath.Value, 18);
        if (AthChangePercentage.HasValue) AthChangePercentage = decimal.Round(AthChangePercentage.Value, 8);
        if (Atl.HasValue) Atl = decimal.Round(Atl.Value, 18);
        if (AtlChangePercentage.HasValue) AtlChangePercentage = decimal.Round(AtlChangePercentage.Value, 8);
    }
}
