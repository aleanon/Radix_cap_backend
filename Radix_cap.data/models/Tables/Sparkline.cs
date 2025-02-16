using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radix_cap.data.models;

[Table("sparklines")]
public class Sparkline
{
    [Key]
    public string AssetId { get; set; }
    public string Prices { get; set; } 
}