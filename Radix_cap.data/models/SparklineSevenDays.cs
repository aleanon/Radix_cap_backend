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
}