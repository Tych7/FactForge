using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DesktopApp;

public class JsonHandler<T>
{
    private readonly JsonSerializerOptions _options;

    public JsonHandler()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true // nice for saving back to file
        };
    }

    public T? LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return default;
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    public void SaveToFile(string filePath, T data)
    {
        var json = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(filePath, json);
        Console.WriteLine($"Data saved to {filePath}");
    }

    public void CreateJsonFile(string filePath)
    {
        File.WriteAllText(filePath, "");
        Console.WriteLine($"New JSON file created at {filePath}");
    }

}
