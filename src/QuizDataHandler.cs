using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DesktopApp;

public static class QuizDataHandler
{
    static QuizDataHandler(){}

    public static void CreateQuiz(string title, List<QuizSlide>? questions = null)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            Directory.CreateDirectory(quizPath);

        var quizData = new QuizData
        {
            Id = GetFirstAvailibleQuizId(),
            Favorite = false,
            Title = title,
            Quiz = questions ?? new List<QuizSlide>(),
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var filePath = Path.Combine(quizPath, $"quiz_{GetFirstAvailibleQuizId()}.json");

        var options = new JsonSerializerOptions { WriteIndented = true };

        var json = JsonSerializer.Serialize(quizData, options);
        File.WriteAllText(filePath, json);

        Console.WriteLine($"Quiz '{title}' created at {filePath}");
    }

    
   public static bool DeleteQuiz(string title)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            return false;

        var filePath = Path.Combine(quizPath, GetFileNameByTitle(title));

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Quiz '{title}' not found at {filePath}");
            return false;
        }

        try
        {
            File.Delete(filePath);
            Console.WriteLine($"Quiz '{title}' deleted successfully.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting quiz '{title}': {ex.Message}");
            return false;
        }
    }

    public static List<QuizSlide> GetAllQuizSlides(string quizTitle)
    {
        var filePath = GetFileNameByTitle(quizTitle);

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine($"Quiz '{quizTitle}' not found.");
            return new List<QuizSlide>();
        }

        try
        {
            var json = File.ReadAllText(filePath);
            var quizData = JsonSerializer.Deserialize<QuizData>(json);

            return quizData?.Quiz ?? new List<QuizSlide>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading quiz '{quizTitle}': {ex.Message}");
            return new List<QuizSlide>();
        }
    }

    public static string GetFileNameByTitle(string title)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            return string.Empty;

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            var extractedTitle = ExtractVarFromFile(file, "title");
            if (extractedTitle == title) return file;
        }

        return string.Empty;
    }

    public static List<string> GetAllQuizTitles()
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        var titles = new List<string>();

        if (!Directory.Exists(quizPath))
            return titles;

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            var extractedTitle = ExtractVarFromFile(file, "title");
            if (!string.IsNullOrEmpty(extractedTitle)) titles.Add(extractedTitle);
        }

        return titles;
    }

    private static int GetFirstAvailibleQuizId()
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        var ids = new List<int>();

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            var extractedId = ExtractVarFromFile(file, "id");
            if (!string.IsNullOrEmpty(extractedId) && int.TryParse(extractedId, out int id))
            {
                ids.Add(id);
            }
        }

        ids.Sort();

        int expected = 1;
        foreach (var id in ids)
        {
            if (id == expected)
            {
                expected++;
            }
            else
            {
                break;
            }
        }

        return expected;
    }


    private static string? ExtractVarFromFile(string file, string varName)
    {
        try
        {
            var json = File.ReadAllText(file);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty(varName, out var element))
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.String:
                        return element.GetString();
                    case JsonValueKind.Number:
                        return element.GetInt32().ToString();
                }
            }
            else
            {
                Console.WriteLine($"No '{varName}' found in {file}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading {file}: {ex.Message}");
        }

        return null;
    }
}
