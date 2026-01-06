using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class QuizLibScrollElement
    {
        public static Border Create(string QuizTitle, Action<string> onQuizClick)
        {
            Border elementBorder = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Avalonia.Thickness(20, 10, 20, 0),
                BorderThickness = new Avalonia.Thickness(0,0,0,2),
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
            };

            Grid mainGrid = new Grid
            {
                Margin = new Avalonia.Thickness(0,0,0,15)
            };

            Button quizButton = new Button
            {
                FontSize = 50,
                Content = QuizTitle,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            quizButton.Click += (_, __) => onQuizClick?.Invoke(QuizTitle);
            mainGrid.Children.Add(quizButton); 


            elementBorder.Child = mainGrid;

            return elementBorder;
        }

    }
}

