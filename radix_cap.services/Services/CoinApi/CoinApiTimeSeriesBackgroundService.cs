using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radix_cap.data.models;

namespace radix_cap.services.Services;

public class CoinApiTimeSeriesBackgroundService : BackgroundService
{
    private readonly ILogger<CoinApiTimeSeriesBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _timeSeriesDelay;
    private readonly DateTime _timeSeriesDefaultStartTime;
    private readonly string _timeSeriesPeriods;

    public CoinApiTimeSeriesBackgroundService(ILogger<CoinApiTimeSeriesBackgroundService> logger,
        IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        
        var timeSeriesDelayInSeconds = configuration.GetValue("ExternalApis:CoinApi:TimeSeriesIntervalSeconds", 5);
        _timeSeriesDelay = TimeSpan.FromSeconds(timeSeriesDelayInSeconds);
        var timeSeriesDefaultStartTime = configuration.GetValue("ExternalApis:CoinApi:PriceHistoryStartTime", DateTime.Now.AddYears(-1));
        _timeSeriesDefaultStartTime = timeSeriesDefaultStartTime;
        var timeSeriesPeriods= configuration.GetValue("ExternalApis:CoinApi:TimeSeriesPeriods", "1HRS");
        _timeSeriesPeriods = timeSeriesPeriods;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var initialScope = _scopeFactory.CreateScope();
        var initialRepo = initialScope.ServiceProvider.GetRequiredService<ICoinApiRepository>();
        var assets = await initialRepo.GetAllAssets();
        var cyclingAssets = CycleThrough(assets);
        
        
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ICoinApiService>();
            var repo = scope.ServiceProvider.GetRequiredService<ICoinApiRepository>();
            
            try
            {
                var asset = cyclingAssets.First();
                cyclingAssets = cyclingAssets.Skip(1);
                
                var timeSeriesStartTime =
                    await repo.GetTimeSeriesStartForAsset(asset, _timeSeriesDefaultStartTime);
                
                var timeSeries = await service.GetTimeSeriesForAsset(asset.Symbol, timeSeriesStartTime, DateTime.Now, _timeSeriesPeriods);

                await repo.SaveTimeSeries(timeSeries, asset.Symbol);
                _logger.LogInformation("Successfully updated database with time series");
                await Task.Delay(_timeSeriesDelay, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching time series");
    
                await Task.Delay(_timeSeriesDelay, stoppingToken);
            }
        }  

    }
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CoinGecko Background Service is stopping.");
        await base.StopAsync(stoppingToken);
    }

    
    private IEnumerable<T> CycleThrough<T>(IEnumerable<T> list)
    {
        while (true)
        {
            foreach (var item in list)
            {
                yield return item;
            }
        }
    }
}