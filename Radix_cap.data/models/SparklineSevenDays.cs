using System.Text.Json;

namespace Radix_cap.data.models;

public class SparklineSevenDays
{
    public decimal[] price {get; set;}

    public static SparklineSevenDays FromPricePoints(PricePoint[] pricePoints)
    {
        return new SparklineSevenDays
        {
            price = pricePoints.Select(p => p.Price).ToArray()
        };
    }

    public static SparklineSevenDays FromDbSparkline(Sparkline? sparkline)
    {
        var price = sparkline == null ? [] : JsonSerializer.Deserialize<decimal[]>(sparkline.Prices);
        return new SparklineSevenDays
        {
            price = price ?? []
        };
    }
}