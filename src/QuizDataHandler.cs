using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DesktopApp;

public static class QuizDataHandler
{
    static QuizDataHandler()
    {

    }

    public static void CreateQuiz(string title, List<QuizQuestion>? questions = null)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            Directory.CreateDirectory(quizPath);

        var quizData = new QuizData
        {
            Title = title,
            Quiz = questions ?? new List<QuizQuestion>()
        };

        var safeFileName = string.Concat(title.Split(Path.GetInvalidFileNameChars())) + ".json";
        var filePath = Path.Combine(quizPath, safeFileName);

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

        var safeFileName = string.Concat(title.Split(Path.GetInvalidFileNameChars())) + ".json";
        var filePath = Path.Combine(quizPath, safeFileName);

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



    public static string GetFileNameByTitle(string title)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            return string.Empty;

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            var extractedTitle = ExtractTitleFromFile(file);
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
            var extractedTitle = ExtractTitleFromFile(file);
            if (!string.IsNullOrEmpty(extractedTitle)) titles.Add(extractedTitle);
        }

        return titles;
    }

    private static string? ExtractTitleFromFile(string file)
    {
        try
        {
            var json = File.ReadAllText(file);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("title", out var titleElement) && titleElement.ValueKind == JsonValueKind.String)
            {
                return titleElement.GetString();
            }
            else
            {
                Console.WriteLine($"No 'title' found in {file}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading {file}: {ex.Message}");
        }

        return null;
    }
}
