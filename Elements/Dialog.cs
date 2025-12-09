using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DesktopApp
{
    public static class Dialog
    {
        public static (Grid dialogGrid, TextBox titleInput) CreateNewQuiz(Grid parentGrid, Action<string> onConfirm)
        {
            int dialogWidth = 600;
            int dialogHeight = 400;
            int edgeSpacing = 20;

            Grid dialogGrid = new Grid { };

            Border darkOverlay = CreateOverlay();
            dialogGrid.Children.Add(darkOverlay);

            Border dialogFrame = AddDialogFrame(dialogHeight, dialogWidth);
            dialogGrid.Children.Add(dialogFrame);

            Grid dialogWindow = AddDialogWindow(dialogHeight, dialogWidth);

            TextBlock header = AddDialogHeader("Add Quiz", edgeSpacing);
            dialogWindow.Children.Add(header);

            var (inputQuizName, titleInput) = AddUserInput("Title", dialogWidth - edgeSpacing * 2); 
            dialogWindow.Children.Add(inputQuizName);

            int buttonPanelHeigth = 100;
            Grid buttonPanel = new Grid
            {
                Width = dialogWidth,
                Height = buttonPanelHeigth,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Avalonia.Thickness(0, 0, 0, edgeSpacing)
            };

            // Close Button
            var closeButton = AddButtonLeft("Close", edgeSpacing, dialogWidth / 2 - edgeSpacing * 1.5);
            closeButton.Click += (sender, e) =>
            {
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(closeButton);

            // Confirm Button
            var confirmButton = AddButtonRight("Confirm", edgeSpacing, dialogWidth / 2 - edgeSpacing * 1.5);
            confirmButton.Click += (sender, e) =>
            {
                onConfirm?.Invoke(titleInput.Text!);
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(confirmButton);
            dialogWindow.Children.Add(buttonPanel);

            dialogGrid.Children.Add(dialogWindow);

            return (dialogGrid, titleInput);
        }


        private static Border CreateOverlay()
        {
            return new Border
            {
                Background = new SolidColorBrush(Color.Parse("#99000000")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }
    
        private static Border AddDialogFrame(int height, int width)
        {
            return new Border
            {
                Classes = { "neon-frame" },
                Height = height,
                Width = width,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private static Grid AddDialogWindow(int height, int width)
        {
            return new Grid
            {
                Height = height,
                Width = width,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private static TextBlock AddDialogHeader(string text, double offset)
        {
            return new TextBlock
            {
                Text = text,
                Classes = {"neon-header"},
                FontSize = 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Avalonia.Thickness(0,offset,0,0)
            };
        }

        private static (Grid container, TextBox input) AddUserInput(string title, double width)
        {
            Grid userInput = new Grid
            {
                Height = 80,
                Width = width
            };

            TextBox inputField = new TextBox
            {
                Classes = {"neon-input"},
                Watermark = $"{title} here",
                Width = width * 2/3,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            userInput.Children.Add(inputField);

            TextBlock fieldTitle = new TextBlock
            {
                Text = $"{title}:",
                Classes = {"neon-text"},
                FontSize = 45,
                Width = width * 1/3,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            userInput.Children.Add(fieldTitle);

            return (userInput, inputField);
        }


        private static Button AddButtonLeft(string content, int edgeMargin, double width)
        {
            return new Button
            {
                Content = content,
                Classes = { "neon-text-button" },
                Margin = new Avalonia.Thickness(edgeMargin, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = width,
                Height = 70
            };
        }

        private static Button AddButtonRight(string content, int edgeMargin, double width)
        {
            return new Button
            {
                Content = content,
                Classes = { "neon-text-button" },
                Margin = new Avalonia.Thickness(0, 0, edgeMargin, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = width,
                Height = 70
            };
        }
    
    }
}