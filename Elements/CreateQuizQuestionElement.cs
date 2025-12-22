
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp;


public static class CreateQuizQuestionElement{

    public static (Border QuizQuestion, TextBox questionText) Create(int questionNumber, string text, int height, VerticalAlignment VerticalAlignment, bool textWrap, string borderColor, string textColor){

        var border = new Border
        {
            Margin = new Thickness(40,60,40,0),
            Height = height,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment,
            BorderBrush = new SolidColorBrush(Color.Parse(borderColor)),
            Background = new SolidColorBrush(Color.Parse("#80000000")),
            CornerRadius = new CornerRadius(10),
            BorderThickness = new Thickness(5),
            BoxShadow = new BoxShadows(
                new BoxShadow
                {
                    Color = Color.Parse(borderColor),
                    Blur = 25,
                    Spread = 5,
                    OffsetX = 0,
                    OffsetY = 0
                }
            )
        };

        TextBox questionText = new TextBox
        {
            Text = text,
            BorderBrush = Brushes.Transparent,
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.Parse(textColor)),
            TextWrapping = TextWrapping.NoWrap,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Left,
            FontWeight = FontWeight.Regular,
            AcceptsReturn = false,
            Margin = new Thickness(20),
        };

        if (textWrap) questionText.Classes.Add("no-scroll");
        
        ElementHander.AutoFitTextBox(questionText, textWrap);

        border.Child = questionText;

        return (border, questionText);
    }
}