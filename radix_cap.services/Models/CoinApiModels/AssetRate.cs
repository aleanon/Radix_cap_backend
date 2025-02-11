using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Radix_cap.data.models;

namespace radix_cap.services.Models.CoinApiModels;

public class AssetRate
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
    [JsonPropertyName("asset_id_quote")]
    public string AssetId { get; set; }
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    public PricePoint IntoPricePoint()
    {
        return new PricePoint
        {
            AssetId = AssetId,
            TimeStamp = Time,
            Price = Rate
        };
    }
}

// public class DecimalJsonConverter : JsonConverter<decimal>
// {
//     public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         if (reader.TokenType == JsonTokenType.Number)
//         {
//             var asDouble = reader.GetDouble();
//             asDouble = Math.Round(asDouble, 18);
//             try
//             {
//                 return new decimal(asDouble);
//             }
//             catch
//             {
//                 Console.WriteLine();
//             }
//         }
//         
//         throw new JsonException($"Unable to convert {reader.GetString()} to decimal");
//     }
//
//     public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
//     {
//         writer.WriteNumberValue(value);
//     }
// }