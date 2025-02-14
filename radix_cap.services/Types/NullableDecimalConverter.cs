using System.Text.Json;
using System.Text.Json.Serialization;

namespace radix_cap.services.Types;

public class NullableDecimalConverter : JsonConverter<decimal?>
{
    public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                try
                {
                    if (reader.TryGetDecimal(out decimal value))
                    {
                        return value;
                    }
                }
                catch
                {
                    return ParseDecimalSafely(reader.GetString());
                }
                break;
            
            case JsonTokenType.String:
                string? stringValue = reader.GetString();
                return ParseDecimalSafely(stringValue);
            
            case JsonTokenType.Null:
                return null;
        }
        
        return null;
    }

    private decimal? ParseDecimalSafely(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        value = value.Trim().Replace(",", "");

        if (value.Contains("e", StringComparison.OrdinalIgnoreCase))
        {
            if (decimal.TryParse(value, System.Globalization.NumberStyles.Float, 
                System.Globalization.CultureInfo.InvariantCulture, out decimal scientificValue))
            {
                return scientificValue;
            }
        }

        if (decimal.TryParse(value, 
            System.Globalization.NumberStyles.Number, 
            System.Globalization.CultureInfo.InvariantCulture, 
            out decimal result))
        {
            return result;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteNumberValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}