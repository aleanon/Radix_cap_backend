using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using radix_cap.services.Types;
using Asset = radix_cap.services.Models.CoinGeckoModels.Asset;

namespace radix_cap.services.Services;

public class CoinGeckoService : ICoinGeckoService 
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinGeckoService> _logger;

    public CoinGeckoService(HttpClient httpClient, IConfiguration configuration, ILogger<CoinGeckoService> logger)
    {
        _httpClient = httpClient;
        var baseUrl = configuration["ExternalApis:CoinGecko:BaseUrl"] ??
                   throw new Exception("Missing BaseUrl for the CoinGecko service");
        _logger = logger;
        
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
        _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        _httpClient.DefaultRequestHeaders.Add("User-Agent","PostmanRuntime/7.43.0");
    }


    public async Task<IEnumerable<Asset>> GetAssets(int page, int pageSize, bool withSparklines)
    {
        try
        {
            var uri = _httpClient.BaseAddress +
                      $"/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={pageSize}&page={page}&sparkline={withSparklines.ToString().ToLower()}"; 
            var response = await _httpClient.GetAsync(uri);
            Console.WriteLine(uri); 

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new NullableDecimalConverter());
            return JsonSerializer.Deserialize<IEnumerable<Asset>>(content, options) ?? throw new NullReferenceException();

        }
        catch (NullReferenceException ex)
        {
            _logger.LogError("Failed to deserialize assets response");
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Failed to fetch assets from CoinAPI");
            throw;
        }  
    }
}