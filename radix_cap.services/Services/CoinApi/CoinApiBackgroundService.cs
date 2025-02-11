using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace radix_cap.services.Services;

public class CoinApiBackgroundService : BackgroundService
{
    private readonly ICoinApiService _coinApiService; 
    private readonly ILogger<CoinApiBackgroundService> _logger;
    private readonly ICoinApiRepository _coinApiRepository;
    private readonly TimeSpan _currentRateDelay;
    private readonly TimeSpan _timeSeriesDelay;
    private readonly DateTime _timeSeriesDefaultStartTime;
    private readonly string _timeSeriesPeriods;

    public CoinApiBackgroundService(ICoinApiService coinApiService, ILogger<CoinApiBackgroundService> logger,
        IConfiguration configuration, ICoinApiRepository coinApiRepository)
    {
        _coinApiService = coinApiService;
        _logger = logger;
        _coinApiRepository = coinApiRepository;
        
        var currentRateDelayInSeconds = configuration.GetValue("ExternalApis:CoinApi:CurrentRatesUpdateInterval", 10);
        _currentRateDelay = TimeSpan.FromSeconds(currentRateDelayInSeconds);
        var timeSeriesDelayInSeconds = configuration.GetValue("ExternalApis:CoinApi:TimeSeriesIntervalSeconds", 5);
        _timeSeriesDelay = TimeSpan.FromSeconds(timeSeriesDelayInSeconds);
        var timeSeriesDefaultStartTime = configuration.GetValue("ExternalApis:CoinApi:PriceHistoryStartTime", DateTime.Now.AddYears(-1));
        _timeSeriesDefaultStartTime = timeSeriesDefaultStartTime;
        var timeSeriesPeriods= configuration.GetValue("ExternalApis:CoinApi:TimeSeriesPeriods", "1HRS");
        _timeSeriesPeriods = timeSeriesPeriods;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentRates = GetCurrentRatesForAllAssets(stoppingToken);
        var getTimeSeries = GetTimeSeries(stoppingToken);
        
        var tasks = new List<Task> { currentRates, getTimeSeries };

        foreach (var task in tasks)
        {
            await task;
        }

    }
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CoinGecko Background Service is stopping.");
        await base.StopAsync(stoppingToken);
    }

    private async Task GetTimeSeries(CancellationToken stoppingToken)
    {
        var assets = await _coinApiRepository.GetAllAssets();
        var cyclingAssets = CycleThrough(assets);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var asset = cyclingAssets.First();
                cyclingAssets = cyclingAssets.Skip(1);
                
                var timeSeriesStartTime =
                   await _coinApiRepository.GetTimeSeriesStartForAsset(asset, _timeSeriesDefaultStartTime);
                
                var timeSeries = await _coinApiService.GetTimeSeriesForAsset(asset.Symbol, timeSeriesStartTime, DateTime.Now, _timeSeriesPeriods);

                await _coinApiRepository.SaveTimeSeries(timeSeries, asset.Symbol);
                _logger.LogInformation("Successfully updated database with time series");
                await Task.Delay(_currentRateDelay, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching time series");
    
                await Task.Delay(_timeSeriesDelay, stoppingToken);
            }
        }  
    }

    private async Task GetCurrentRatesForAllAssets(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Fetching current rates from CoinApi");
                
                var currentRates = await _coinApiService.GetCurrentRatesForAllAssets();
                
                _logger.LogInformation("Successfully fetched current rates for all assets");

                await _coinApiRepository.SaveCurrentRates(currentRates);

                _logger.LogInformation("Successfully updated database with latest rates");
                await Task.Delay(_currentRateDelay, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching assets");

                await Task.Delay(_currentRateDelay, stoppingToken);
            }
        } 
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