using System;
using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class QuizLibDetailsElement
    {
        public static string? selectedQuizTitle;

        public static Border Create(
            int width,
            Action onDeleteClick,
            Action onEditClick,
            Action onStartClick)
        {
            //RIGHT SIDE
            Border rightSide = new Border
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = width,
                Classes = { "neon-frame" },
            };

            Grid detailsGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            rightSide.Child = detailsGrid;

            var detailsQuizTitle = new TextBlock
            {
                Text = selectedQuizTitle,
                FontSize = 48,
                Classes = { "neon-text" },
                TextWrapping = TextWrapping.NoWrap,
                TextAlignment = TextAlignment.Left
            };

            var detailsQuizTitleViewBox = new Viewbox
            {
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.DownOnly,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Child = detailsQuizTitle
            };

            Border detailsQuizTitleUnderline = new Border
            {
                Height = 80,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 0, 20, 0),
                BorderBrush = SolidColorBrush.Parse("#00FFFF"),
                BorderThickness = new Thickness(0, 0, 0, 2),
                Child = detailsQuizTitleViewBox
            };
            detailsGrid.Children.Add(detailsQuizTitleUnderline);


            StackPanel detailsButtons = CreateDetailsButtons(onDeleteClick, onEditClick, onStartClick);
            detailsGrid.Children.Add(detailsButtons);

            return rightSide;
        }

        private static StackPanel CreateDetailsButtons(Action onDeleteClick, Action onEditClick, Action onStartClick)
        {
            StackPanel buttonUnit = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0,0,0,20),
                Spacing = 10
            };

            Button startButton = new Button
            {
                Content = "START",
                Height = 80,
                Width = 200,
                Classes = {"neon-text-button"},
                Foreground = SolidColorBrush.Parse("#8C52FF"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };
            startButton.Click += (_, __) => onStartClick?.Invoke();
            buttonUnit.Children.Add(startButton);

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
                Height = 80,
                Width = 80,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Content = new Border { Child = editIcon },                
            };
            editButton.Click += (_, __) => onEditClick?.Invoke();
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
                Height = 80,
                Width = 80,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Content = new Border { Child = deleteButtonIcon },
            };
            deleteButton.Click += (_, __) => onDeleteClick?.Invoke();
            buttonUnit.Children.Add(deleteButton);

            return buttonUnit;
        }
    }
}