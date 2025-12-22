using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Controls;

public record CurrentSlideData
{

    public int Id { get; set; }

    public string? Type { get; set; }

    public TextBox? Question { get; set; }

    public List<TextBox>? Answers { get; set; }

    public TextBox? CorrectAnswer { get; set; }

    public ComboBox? Time { get; set; }

    public string? BgImagePath { get; set; }

    public string? Category { get; set; }

    public string? ImagePath { get; set; }

    public string? AudioPath { get; set; }
    
}