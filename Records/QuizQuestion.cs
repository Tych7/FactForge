using System.Collections.Generic;

public class QuizQuestion
{
    public enum QuizTypes
    {
        multiple,
        open
    }

    public int Number { get; set; }
    public string? Type { get; set; } // "multiple" or "open"
    public string? Question { get; set; }
    public List<string>? Answers { get; set; }
    public string? CorrectAnswer { get; set; }
    public int? Time { get; set; }
}