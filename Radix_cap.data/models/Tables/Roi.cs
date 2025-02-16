using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radix_cap.data.models;

[Table("rois")]
public class Roi
{
    [Key]
    public string AssetId { get; set; }
    public double? Times { get; set; }
    public string? Currency  { get; set; }
    public double? Percentage { get; set; } 
}