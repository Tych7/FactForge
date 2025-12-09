using System;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace DesktopApp
{
    public static class QuizLibScrollElement
    {
        public static Border Create(string QuizTitle, Action onDelete, Action onEdit)
        {
            Border elementBorder = new Border
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Margin = new Avalonia.Thickness(10, 10, 10, 0),
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
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left
            };
            mainGrid.Children.Add(quizTitle); 


            StackPanel buttonUnit = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
            };

            var editIcon = new Uri("avares://DesktopApp/Assets/Icons/pencil.png");
            Image editButtonIcon = new Image
            {
                Source = new Bitmap(AssetLoader.Open(editIcon)),
                Stretch = Stretch.Uniform,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            };
            Button editButton = new Button
            {
                Height = 60,
                Width = 60,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Content = editButtonIcon,
                Margin = new Avalonia.Thickness(0,0,10,0)
            };
            editButton.Click += (sender, e) =>
            {
                onEdit?.Invoke();
            };
            buttonUnit.Children.Add(editButton);

            var deleteIcon = new Uri("avares://DesktopApp/Assets/Icons/trash.png");
            Image deleteButtonIcon = new Image
            {
                Source = new Bitmap(AssetLoader.Open(deleteIcon)),
                Stretch = Stretch.Uniform,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            };
            Button deleteButton = new Button
            {
                Height = 60,
                Width = 60,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Content = deleteButtonIcon
            };
            deleteButton.Click += (sender, e) =>
            {
                onDelete?.Invoke();
            };
            buttonUnit.Children.Add(deleteButton);

            mainGrid.Children.Add(buttonUnit);

            elementBorder.Child = mainGrid;

            return elementBorder;
        }

    }
}

