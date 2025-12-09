using System.Collections.Generic;
using Avalonia.Controls;

namespace DesktopApp;

public class OpenQuestionElement
{
    public Grid Create(string? question = default, List<string>? awnsers = default, int? time = default, QuizQuestion.QuizTypes? type = default)
    {
        var page = new Grid
        {
            Height=972,
            Width =1728,
        };

        var options = new Grid
        {
            Height = 600,
            Width = 1728,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        page.Children.Add(options);

        var option1 = new TextBox
        {
            
        };

        return page;
    }
}