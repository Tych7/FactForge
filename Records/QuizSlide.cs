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
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SlideTypes? Type { get; set; }

    [JsonPropertyName("header")]
    public QuizSlideText? Header { get; set; }

    [JsonPropertyName("subText")]
    public QuizSlideText? SubText { get; set; }

    [JsonPropertyName("question")]
    public string? Question { get; set; }

    [JsonPropertyName("answers")]
    public List<string>? Answers { get; set; }

    [JsonPropertyName("correctAnswer")]
    public string? CorrectAnswer { get; set; }

    [JsonPropertyName("time")]
    public int? Time { get; set; }

    [JsonPropertyName("bgImagePath")]
    public string? BgImagePath { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("imagePath")]
    public string? ImagePath { get; set; }

    [JsonPropertyName("audioPath")]
    public string? AudioPath { get; set; }


    public record QuizSlideText
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("fontSize")]
        public int FontSize { get; set; }
    }
   
}