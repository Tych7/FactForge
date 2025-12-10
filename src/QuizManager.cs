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
        LoadQuestions("Test Quiz");
    }

    public static void LoadQuestions(string quizName)
    {
        string quizFileName = QuizDataHandler.GetFileNameByTitle(quizName);
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Quizzen", quizFileName);
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

}
