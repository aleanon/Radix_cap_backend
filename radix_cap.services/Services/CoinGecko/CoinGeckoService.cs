using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Asset = radix_cap.services.Models.CoinGeckoModels.Asset;

namespace radix_cap.services.Services;

public class CoinGeckoService : ICoinGeckoService 
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CoinGeckoService> _logger;
    private readonly string _baseUrl;

    public CoinGeckoService(HttpClient httpClient, IConfiguration configuration, ILogger<CoinGeckoService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _baseUrl = _configuration["ExternalApis:CoinGecko:BaseUrl"] ??
                   throw new Exception("Missing BaseUrl for the CoinGecko service");
        _logger = logger;
        
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
        _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        _httpClient.DefaultRequestHeaders.Add("User-Agent","PostmanRuntime/7.43.0");
    }


    public async Task<IEnumerable<Asset>> GetAssets(int page, int pageSize, bool withSparklines = false)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                _httpClient.BaseAddress + $"/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={pageSize}&page={page}&sparkline={withSparklines}"
            );

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Asset>>(content) ?? throw new NullReferenceException();

        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine("Failed to deserialize assets response");
            throw;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Failed to fetch assets from CoinAPI");
            throw;
        }  
    }
}