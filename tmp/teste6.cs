using System;
using System.Text.Json;

public class Program
{
    public static string? GetValueFromPath(JsonElement root, string path)
    {
        // Divide o caminho em propriedades separadas por ":"
        var segments = path.Split(':', StringSplitOptions.RemoveEmptyEntries);

        JsonElement current = root;

        foreach (var segment in segments)
        {
            if (segment.StartsWith("[") && segment.EndsWith("]"))
            {
                // Trata o caso de arrays (ex: [0])
                if (current.ValueKind == JsonValueKind.Array)
                {
                    // Extrai o índice do formato [n]
                    if (int.TryParse(segment.Trim('[', ']'), out int index))
                    {
                        if (index >= 0 && index < current.GetArrayLength())
                        {
                            current = current[index];
                        }
                        else
                        {
                            // Índice fora do limite
                            return null;
                        }
                    }
                    else
                    {
                        // Índice inválido
                        return null;
                    }
                }
                else
                {
                    // Não é um array
                    return null;
                }
            }
            else
            {
                // Trata o caso de objetos
                if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(segment, out current))
                {
                    continue;
                }
                else
                {
                    // Propriedade não encontrada
                    return null;
                }
            }
        }

        // Retorna o valor encontrado baseado no tipo
        return current.ValueKind switch
        {
            JsonValueKind.String => current.GetString(),
            JsonValueKind.Number => current.GetRawText(),
            JsonValueKind.True or JsonValueKind.False => current.GetBoolean().ToString(),
            JsonValueKind.Null => "null",
            _ => current.GetRawText()
        };
    }

    public static void Main()
    {
        string json = @"
        {
          ""User"": {
            ""Name"": ""John"",
            ""Age"": 30,
            ""Address"": [
              {
                ""City"": ""New York"",
                ""ZipCode"": ""10001""
              },
              {
                ""City"": ""Los Angeles"",
                ""ZipCode"": ""90001""
              }
            ]
          }
        }";

        using (JsonDocument document = JsonDocument.Parse(json))
        {
            JsonElement root = document.RootElement;

            // Testa obtenção de valores em subníveis com arrays
            Console.WriteLine($"City[0]: {GetValueFromPath(root, "User:Address:[0]:City")}"); // New York
            Console.WriteLine($"ZipCode[1]: {GetValueFromPath(root, "User:Address:[1]:ZipCode")}"); // 90001

            // Testa caminhos inexistentes
            Console.WriteLine($"City[2]: {GetValueFromPath(root, "User:Address:[2]:City")}"); // null
            Console.WriteLine($"Invalid Path: {GetValueFromPath(root, "User:Address:[0]:Country")}"); // null

            // Testa valores fora de contexto
            Console.WriteLine($"Invalid Index: {GetValueFromPath(root, "User:Name:[0]")}"); // null
        }
    }
}












using System;
using System.Text.Json;

public class Program
{
    public static string? GetValueFromPath(JsonElement root, string path)
    {
        // Divide o caminho em propriedades separadas por ":"
        var properties = path.Split(':', StringSplitOptions.RemoveEmptyEntries);

        JsonElement current = root;

        // Navegar pelos subníveis
        foreach (var property in properties)
        {
            if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(property, out current))
            {
                continue; // Se encontrou a propriedade, continua navegando
            }
            else
            {
                return null; // Propriedade não encontrada
            }
        }

        // Retorna o valor encontrado baseado no tipo
        return current.ValueKind switch
        {
            JsonValueKind.String => current.GetString(),
            JsonValueKind.Number => current.GetRawText(),
            JsonValueKind.True or JsonValueKind.False => current.GetBoolean().ToString(),
            JsonValueKind.Null => "null",
            _ => current.GetRawText() // Para objetos ou arrays, retorna o JSON bruto
        };
    }

    public static void Main()
    {
        string json = @"
        {
          ""User"": {
            ""Name"": ""John"",
            ""Age"": 30,
            ""Address"": {
              ""City"": ""New York"",
              ""ZipCode"": ""10001""
            }
          },
          ""Settings"": {
            ""Theme"": ""Dark"",
            ""Notifications"": {
              ""Email"": true,
              ""SMS"": false
            }
          }
        }";

        using (JsonDocument document = JsonDocument.Parse(json))
        {
            JsonElement root = document.RootElement;

            // Testa obtenção de valores em subníveis
            Console.WriteLine($"Name: {GetValueFromPath(root, "User:Name")}"); // John
            Console.WriteLine($"City: {GetValueFromPath(root, "User:Address:City")}"); // New York
            Console.WriteLine($"Email Notifications: {GetValueFromPath(root, "Settings:Notifications:Email")}"); // True

            // Testa propriedades inexistentes
            Console.WriteLine($"Country: {GetValueFromPath(root, "User:Address:Country")}"); // null
            Console.WriteLine($"Invalid Path: {GetValueFromPath(root, "User:Invalid:Path")}"); // null
        }
    }
}
