using Microsoft.EntityFrameworkCore;

namespace Radix_cap.data.models;

[PrimaryKey(nameof(asset_name), nameof(asset_ticker), nameof(timestamp))]
public class Price
{
    public string asset_name { get; set; }
    public string asset_ticker { get; set; }
    public DateTime timestamp { get; set; }
    public decimal price { get; set; }
}