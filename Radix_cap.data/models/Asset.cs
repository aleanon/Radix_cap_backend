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
    public string Image { get; set; }

    public decimal CurrentPrice { get; set; }

    public long MarketCap { get; set; }

    public int MarketCapRank { get; set; }

    public long FullyDilutedValuation { get; set; }

    public double TotalVolume { get; set; }

    public double? High24H { get; set; }

    public double? Low24H { get; set; }

    public double? PriceChange24H { get; set; }

    public double? PriceChangePercentage24H { get; set; }

    public double? MarketCapChange24H { get; set; }

    public double? MarketCapChangePercentage24H { get; set; }

    public double CirculatingSupply { get; set; }

    public double TotalSupply { get; set; }

    public double? MaxSupply { get; set; }

    public double Ath { get; set; }

    public double AthChangePercentage { get; set; }

    public DateTime AthDate { get; set; }

    public double Atl { get; set; }

    public double AtlChangePercentage { get; set; }

    public DateTime AtlDate { get; set; }

    public DateTime LastUpdated { get; set; }
}
