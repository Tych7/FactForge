
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using HarfBuzzSharp;

namespace DesktopApp;


public static class QuizSlideElements{

    public static (Border SlideQuestionElement, TextBox questionText) CreateQuestion(int questionNumber, string text, int height, VerticalAlignment VerticalAlignment, bool textWrap, string borderColor, string textColor){

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

        TextBox headerText = new TextBox
        {
            Text = text,
            Watermark = "Question here...",
            BorderBrush = Brushes.Transparent,
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.Parse(textColor)),
            TextWrapping = TextWrapping.NoWrap,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Left,
            FontWeight = FontWeight.Regular,
            AcceptsReturn = false,
            Margin = new Thickness(20),
            Effect = new DropShadowEffect
            {
                Color = Color.Parse(textColor),
                BlurRadius = 15,
                Opacity = 1,
                OffsetX = -1,
                OffsetY = -1
            }
        };

        if (textWrap) headerText.Classes.Add("no-scroll");
        
        ElementHander.AutoFitTextBox(headerText, textWrap);

        border.Child = headerText;

        return (border, headerText);
    }

    public static TextBox CreateHeader(string text, int fontSize, bool textWrap)
    {
        TextBox headerText = new TextBox
        {
            Text = text,
            FontSize = fontSize,
            Watermark = "Header here...",
            BorderBrush = Brushes.Transparent,
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.Parse("#C0A2FC")),
            TextWrapping = textWrap ? TextWrapping.Wrap : TextWrapping.NoWrap,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            FontFamily = (FontFamily)Application.Current?.Resources["OrbitronBold"]!,
            AcceptsReturn = false,
            Effect = new DropShadowEffect
            {
                Color = Color.Parse("#8C52FF"),
                BlurRadius = 25,
                Opacity = 1,
                OffsetX = -1,
                OffsetY = -1
            }
        };


        if (textWrap) headerText.Classes.Add("no-scroll");

        return headerText;
    }

    public static TextBox CreateSubText(string text, int fontSize, bool textWrap)
    {
        TextBox subText = new TextBox
        {
            Text = text,
            FontSize = fontSize,
            Watermark = "Subtext here...",
            BorderBrush = Brushes.Transparent,
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.Parse("#AEF5F5")),
            TextWrapping = textWrap ? TextWrapping.Wrap : TextWrapping.NoWrap,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            FontFamily = (FontFamily)Application.Current?.Resources["OrbitronRegular"]!,
            AcceptsReturn = false,
            Effect = new DropShadowEffect
            {
                Color = Color.Parse("#00FFFF"),
                BlurRadius = 25,
                Opacity = 1,
                OffsetX = -1,
                OffsetY = -1
            }
        };


        if (textWrap) subText.Classes.Add("no-scroll");
        
        return subText;
    }
}