using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DesktopApp;

public static class QuizManager
{
    private static List<QuizQuestion>? _questions;
    private static int _currentIndex = 0;

    static QuizManager()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Quizzen", "quiz_1.json");
        LoadQuestions(filePath);
    }


    public static void LoadQuestions(string filePath)
    {
        Console.WriteLine($"Loading quiz from: {filePath}");

        var handler = new JsonHandler<QuizData>();
        var data = handler.LoadFromFile(filePath);

        if (data?.Quiz == null || data.Quiz.Count == 0)
        {
            Console.WriteLine("No questions found in JSON.");
            _questions = new List<QuizQuestion>();
            return;
        }

        _questions = data.Quiz;
        _currentIndex = 0;
        Console.WriteLine($"Loaded {_questions.Count} questions.");
    }


    public static QuizQuestion GetNextQuestion()
    {
        if (_questions == null || _questions.Count == 0)
            throw new InvalidOperationException("No questions loaded.");

        var question = _questions[_currentIndex];
        _currentIndex = (_currentIndex + 1) % _questions.Count;
        return question;
    }

    public static List<string> GetAllQuizTitles()
    {
        var quizPath = Path.Combine(AppContext.BaseDirectory, "Data", "Quizzen");
        var titles = new List<string>();

        if (!Directory.Exists(quizPath))
            return titles;

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            try
            {
                var json = File.ReadAllText(file);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("title", out var titleElement) && titleElement.ValueKind == JsonValueKind.String)
                {
                    titles.Add(titleElement.GetString()!);
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
        }

        return titles;
    }

}
