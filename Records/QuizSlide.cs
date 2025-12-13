using System.Collections.Generic;
using System.Text.Json.Serialization;

public record QuizSlide
{
    public enum SlideTypes
    {
        MultipleChoiceQuestion,
        OpenQuestion,
        Text

    }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("question")]
    public string? Question { get; set; }

    [JsonPropertyName("answers")]
    public List<string>? Answers { get; set; }

    [JsonPropertyName("correctAnswer")]
    public string? CorrectAnswer { get; set; }

    [JsonPropertyName("time")]
    public int? Time { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }
    
}