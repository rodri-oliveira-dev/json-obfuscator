using System;
using System.Text.Json;

public static class JsonExtensions
{
    public static T? GetValue<T>(this JsonElement element)
    {
        try
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => (T)(object)element.GetString(),
                JsonValueKind.Number when typeof(T) == typeof(int) => (T)(object)element.GetInt32(),
                JsonValueKind.Number when typeof(T) == typeof(double) => (T)(object)element.GetDouble(),
                JsonValueKind.True or JsonValueKind.False when typeof(T) == typeof(bool) => (T)(object)element.GetBoolean(),
                JsonValueKind.Null => default,
                _ => throw new InvalidOperationException($"Unsupported type {typeof(T)} for JSON value.")
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error converting JSON element to {typeof(T)}: {ex.Message}", ex);
        }
    }
}

class Program
{
    public static void Main()
    {
        string json = "{ \"Name\": \"John\", \"Age\": 30, \"IsActive\": true }";

        using (JsonDocument document = JsonDocument.Parse(json))
        {
            JsonElement root = document.RootElement;

            if (root.TryGetProperty("Name", out JsonElement nameProperty))
            {
                string name = nameProperty.GetValue<string>();
                Console.WriteLine($"Name: {name}");
            }

            if (root.TryGetProperty("Age", out JsonElement ageProperty))
            {
                int age = ageProperty.GetValue<int>();
                Console.WriteLine($"Age: {age}");
            }

            if (root.TryGetProperty("IsActive", out JsonElement isActiveProperty))
            {
                bool isActive = isActiveProperty.GetValue<bool>();
                Console.WriteLine($"IsActive: {isActive}");
            }
        }
    }
}
