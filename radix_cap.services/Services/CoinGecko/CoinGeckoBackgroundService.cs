using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace radix_cap.services.Services;

public class CoinGeckoBackgroundService : BackgroundService
{
    private readonly ILogger<CoinGeckoBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _delay;
    private int _retryAttempts = 0;
    private int _currentPage = 1;

    public CoinGeckoBackgroundService(
        ILogger<CoinGeckoBackgroundService> logger,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        var seconds = configuration.GetValue("ExternalApis:CoinGecko:UpdateIntervalSeconds", 60);
        _delay = TimeSpan.FromSeconds(seconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ICoinGeckoService>();
            var repo = scope.ServiceProvider.GetRequiredService<ICoinGeckoRepository>();
            if (_retryAttempts >= 3)
            {
                _currentPage += 1;
                _retryAttempts = 0;
            }
            
            try
            {
                _logger.LogInformation("Fetching assets from CoinGecko");
                
                var assets = await service.GetAssets(_currentPage, 100, true);
                
                _logger.LogInformation("Successfully fetched {assetsCount} assets, saving to database", assets.Count());

                await repo.SaveAssets(assets);

                _logger.LogInformation("Successfully updated database with latest assets for page: {page}", _currentPage);
                _currentPage++;
                await Task.Delay(_delay, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching assets");

                _retryAttempts += 1;
                await Task.Delay(_delay, stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CoinGecko Background Service is stopping.");
        await base.StopAsync(stoppingToken);
    }
}