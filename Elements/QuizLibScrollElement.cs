using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class QuizLibScrollElement
    {
        public static Border Create(string QuizTitle, Action<string> onDelete, Action<string> onEdit)
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

            TextBlock quizTitle = new TextBlock
            {
                Classes = { "neon-text" },
                FontSize = 50,
                Text = QuizTitle,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            mainGrid.Children.Add(quizTitle); 


            StackPanel buttonUnit = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            
            var editIcon = new PathIcon
            {
                Data = AppHandler.GetIcon("edit_regular"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 30,
                Width = 30
            };
            Button editButton = new Button
            {
                Height = 60,
                Width = 60,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Content = new Border { Child = editIcon },                
                Margin = new Avalonia.Thickness(0,0,10,0)
            };
            editButton.Click += (_, __) => onEdit?.Invoke(QuizTitle);
            buttonUnit.Children.Add(editButton);

            var deleteButtonIcon = new PathIcon
            {
                Data = AppHandler.GetIcon("delete_regular"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 30,
                Width = 30
            };
            Button deleteButton = new Button
            {
                Height = 60,
                Width = 60,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Content = new Border { Child = deleteButtonIcon },
            };
            deleteButton.Click += (_, __) => onDelete?.Invoke(QuizTitle);
            buttonUnit.Children.Add(deleteButton);

            mainGrid.Children.Add(buttonUnit);

            elementBorder.Child = mainGrid;

            return elementBorder;
        }

    }
}

