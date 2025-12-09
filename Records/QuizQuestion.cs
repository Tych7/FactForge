using System.Collections.Generic;

public record QuizQuestion
{
    public enum QuizTypes
    {
        multiple,
        open
    }

    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Question { get; set; }
    public List<string>? Answers { get; set; }
    public string? CorrectAnswer { get; set; }
    public int? Time { get; set; }
    public string? Category { get; set; }
}