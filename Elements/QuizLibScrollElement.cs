using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class QuizLibScrollElement
    {
        public static Border Create(int id, string QuizTitle, Action<int, string> onQuizClick)
        {
            Border elementBorder = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Avalonia.Thickness(20, 10, 20, 0),
                BorderThickness = new Avalonia.Thickness(0,0,0,2),
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
            };

            Button quizButton = new Button
            {
                Name = id.ToString(),
                Height = 60,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0,0,0,15),
                FontSize = 50,
                Content = QuizTitle,
                Classes = {"neon-text-button"},
                BorderThickness = new Avalonia.Thickness(0),
                Foreground = SolidColorBrush.Parse("#8C52FF"),
                BorderBrush = new SolidColorBrush(Color.Parse("#FFFFFF")),
                Effect = new DropShadowEffect
                {
                    Color = Color.Parse("#00000000"),
                    BlurRadius = 0,
                    OffsetX = 0,
                    OffsetY = 0,
                    Opacity = 0
                }

            };
            quizButton.Click += (_, __) => onQuizClick?.Invoke(id, QuizTitle);

            elementBorder.Child = quizButton;

            return elementBorder;
        }

    }
}

