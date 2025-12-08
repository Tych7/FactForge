using System.Collections.Generic;
using System.Text.Json.Serialization;

public class QuizData
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("quiz")]
    public required List<QuizQuestion> Quiz { get; set; }
}
