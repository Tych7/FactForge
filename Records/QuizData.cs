using System.Collections.Generic;
using System.Text.Json.Serialization;

public record QuizData
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("favorite")]
    public required bool Favorite { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("quiz")]
    public required List<QuizQuestion> Quiz { get; set; }

    [JsonPropertyName("timestamp")]
    public required string Timestamp { get; set; }
}
