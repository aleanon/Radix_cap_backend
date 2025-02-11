using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;

namespace Radix_cap.data.models;

public class Coin
{
    [Key]
    public string Ticker { get; set; }
    public string Name { get; set; }
    public long circulating_supply { get; set; }
    public long max_supply { get; set; }
    public long total_supply { get; set; }
}