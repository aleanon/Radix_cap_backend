using System.Diagnostics;
using System.Text.Json;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using radix_cap.services.Models.CoinApiModels;

namespace radix_cap.services.Services;


public class CoinApiService : ICoinApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinApiService> _logger;

    public CoinApiService(HttpClient httpClient, IConfiguration configuration, ILogger<CoinApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        var baseUrl = configuration["ExternalApis:CoinAPi:BaseUrl"] ??
                   throw new Exception("Missing BaseUrl for the CoinApi service");
        
        var apiKey = configuration["ExternalApis:CoinAPi:ApiKey"] ?? throw new Exception("Missing ApiKey for the CoinApi service");

        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "text/plain");
        _httpClient.DefaultRequestHeaders.Add("X-CoinAPI-Key", apiKey);
    }

    public async Task<IEnumerable<Asset>> GetAllAssets()
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"/v1/assets"
            );

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Asset>>(content) ?? throw new NullReferenceException();

        }
        catch (NullReferenceException ex)
        {
            _logger.LogError("Failed to deserialize assets response: {ex}", ex);
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Failed to fetch assets from CoinAPI: {ex}", ex);
            throw;
            
        } 
    }

    public async Task<IEnumerable<TimeSerie>> GetTimeSeriesForAsset(string assetSymbol, DateTime startTime, DateTime endTime, string period)
    {

        try
        {
            var requestUri =
                $"/v1/exchangerate/{assetSymbol}/USD/history?period_id={period}&time_start={startTime:yyyy-MM-ddTHH:mm:ss.fffffffZ}&time_end={endTime:yyyy-MM-ddTHH:mm:ss.fffffffZ}&limit=10000"; 
            var response = await _httpClient.GetAsync(requestUri);

            _logger.LogInformation("Status code time series response {}", response.StatusCode);
            
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<TimeSerie>>(content) ?? throw new NullReferenceException();
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError(ex, "Failed to deserialize TimeSeries response for asset: {assetId}", assetSymbol);
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch TimeSeries from CoinAPI for asset: {assetId}", assetSymbol);
            throw;
        } 
    }

    public async Task<CurrentRates> GetCurrentRatesForAllAssets()
    {
        try
        {
            var response = await _httpClient.GetAsync(
                "/v1/exchangerate/USD?invert=true"
            );
    
            response.EnsureSuccessStatusCode();
    
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CurrentRates>(content) ?? throw new NullReferenceException();
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError("Failed to deserialize current rates");
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Failed to fetch current rates from CoinAPI");
            throw;
        }  
    }
}