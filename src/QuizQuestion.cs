using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class QuizQuestion
{
    public int Number { get; set; }
    public string? Type { get; set; } // "multiple" or "open"
    public string? Question { get; set; }
    public List<string>? Answers { get; set; }
    public string? CorrectAnswer { get; set; }
}

public class QuizData
{
    public List<QuizQuestion>? Quiz { get; set; }
}

public static class QuizManager
{
    private static List<QuizQuestion>? _questions;
    private static int _currentIndex = 0;

    static QuizManager()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "quizzen", "quiz.json");
        LoadQuestions(filePath);
    }


    public static void LoadQuestions(string filePath)
    {
        Console.WriteLine($"Loading quiz from: {filePath}");

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found!");
            return;
        }

        var json = File.ReadAllText(filePath);
        Console.WriteLine("File contents:");
        Console.WriteLine(json);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<QuizData>(json, options);

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

}
