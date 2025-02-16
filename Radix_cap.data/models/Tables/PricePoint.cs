using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Radix_cap.data.models;
[Table("prices")]
[PrimaryKey(nameof(AssetId), nameof(TimeStamp))]
public class PricePoint
{
    public string AssetId { get; set; }
    public DateTime TimeStamp { get; set; }
    public decimal Price { get; set; }
}