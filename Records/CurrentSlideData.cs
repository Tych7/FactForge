using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using static QuizSlide;

public record CurrentSlideData
{

    public int Id { get; set; }

    public string? Type { get; set; }

    public TextBox? Header { get; set; }

    public TextBox? SubText { get; set; }

    public TextBox? Question { get; set; }

    public List<TextBox>? Answers { get; set; }

    public TextBox? CorrectAnswer { get; set; } // for open questions
    public ComboBox? CorrectAnswerComboBox { get; set; } // for multiple choice

    public ComboBox? Time { get; set; }

    public string? BgImagePath { get; set; }

    public string? Category { get; set; }

    public string? ImagePath { get; set; }

    public string? AudioPath { get; set; }

    public string? GetCurrentCorrectAnswer()
    {
        if (Type == SlideTypes.MultipleChoiceQuestion.ToString())
            return CorrectAnswerComboBox?.SelectedValue?.ToString();
        else
            return CorrectAnswer?.Text;
    }
    
}