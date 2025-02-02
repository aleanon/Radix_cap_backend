using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;

namespace Radix_cap.data.models;

[PrimaryKey(nameof(Name), nameof(Ticker))]
public class Coin
{
    public string Name { get; set; }
    public string Ticker { get; set; }
    public long circulating_supply { get; set; }
    public long max_supply { get; set; }
    public long total_supply { get; set; }
}