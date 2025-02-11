namespace radix_cap.services.Models.CoinApiModels;

public enum Periods
{
    EveryFiveSeconds,
    EveryHour,
}


public static class PeriodsExtensions
{
    public static string ToString(this Periods period)
    {
        return period switch
        {
            Periods.EveryFiveSeconds => "5SEC",
            Periods.EveryHour => "1HRS",
            _ => "Unknown Period"
        };
    }
}