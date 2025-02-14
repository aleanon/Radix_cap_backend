using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace radix_cap.services.Services;

public class CoinApiCurrentPricesBackgroundService : BackgroundService
{
    private readonly ILogger<CoinApiCurrentPricesBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _currentRateDelay;

    public CoinApiCurrentPricesBackgroundService(ILogger<CoinApiCurrentPricesBackgroundService> logger,
        IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        
        var currentRateDelayInSeconds = configuration.GetValue("ExternalApis:CoinApi:CurrentRatesUpdateInterval", 10);
        _currentRateDelay = TimeSpan.FromSeconds(currentRateDelayInSeconds);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ICoinApiService>();
            var repo = scope.ServiceProvider.GetRequiredService<ICoinApiRepository>();
            
            try
            {
                _logger.LogInformation("Fetching current rates from CoinApi");
                
                var currentRates = await service.GetCurrentRatesForAllAssets();
                
                _logger.LogInformation("Successfully fetched current rates for all assets");

                await repo.SaveCurrentRates(currentRates);

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
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CoinGecko Background Service is stopping.");
        await base.StopAsync(stoppingToken);
    }
}