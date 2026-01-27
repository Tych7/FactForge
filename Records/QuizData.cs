using System.Collections.Generic;
using System.Text.Json.Serialization;

public record QuizData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("favorite")]
    public bool Favorite { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("quiz")]
    public List<QuizSlide>? Quiz { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }
}
