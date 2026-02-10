using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Tmds.DBus.Protocol;

namespace DesktopApp
{
    public static class Dialog
    {
        public static Grid SelectOrUploadSlideImage(Grid parentGrid, Action<string> onConfirm)
        {
            int dialogWidth = 1200;
            int dialogHeight = 800;
            int edgeSpacing = 20;

            Grid dialogGrid = new Grid { };

            Border darkOverlay = CreateOverlay();
            dialogGrid.Children.Add(darkOverlay);

            Border dialogFrame = AddDialogFrame(dialogHeight, dialogWidth);
            dialogGrid.Children.Add(dialogFrame);
            
            Grid dialogWindow = AddDialogWindow(dialogHeight, dialogWidth);

            TextBlock dialogHeader = AddDialogHeader("Select an Image", edgeSpacing);
            dialogWindow.Children.Add(dialogHeader);

            int buttonPanelHeigth = 100;
            Grid buttonPanel = new Grid
            {
                Width = dialogWidth,
                Height = buttonPanelHeigth,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Avalonia.Thickness(0, 0, 0, edgeSpacing)
            };

            Grid ImageSelection = AddImageSelection(QuizDataHandler.GetAllSlideImagePaths());
            dialogWindow.Children.Add(ImageSelection);

            // Close Button
            var UploadNewButton = AddButton("Upload New", edgeSpacing, HorizontalAlignment.Left, dialogWidth / 2 - edgeSpacing * 1.5);
            UploadNewButton.Click += async (sender, e) =>
            {
                await QuizDataHandler.PickAndStoreImage();
                parentGrid.Children.Remove(dialogGrid);
                SelectOrUploadSlideImage(parentGrid, onConfirm);
            };
            buttonPanel.Children.Add(UploadNewButton);

            // Confirm Button
            var confirmButton = AddButton("Confirm", edgeSpacing, HorizontalAlignment.Right, dialogWidth / 2 - edgeSpacing * 1.5);
            confirmButton.Click += (sender, e) =>
            {
                onConfirm?.Invoke(_selectedButton?.Name ?? string.Empty);
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(confirmButton);
            dialogWindow.Children.Add(buttonPanel);

            dialogGrid.Children.Add(dialogWindow);
            return dialogGrid;
        }

        public static Grid AreYouSure(Grid parentGrid, string message, Action onConfirm)
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

            TextBlock dialogHeader = AddDialogHeader("Are you sure?", edgeSpacing);
            dialogWindow.Children.Add(dialogHeader);

            Grid dialogMessage = AddDynamicMessage(message, dialogHeight - 200, dialogWidth - edgeSpacing * 2);
            dialogWindow.Children.Add(dialogMessage);

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
            var closeButton = AddButton("Close", edgeSpacing, HorizontalAlignment.Left, dialogWidth / 2 - edgeSpacing * 1.5);
            closeButton.Click += (sender, e) =>
            {
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(closeButton);

            // Confirm Button
            var confirmButton = AddButton("Confirm", edgeSpacing, HorizontalAlignment.Right, dialogWidth / 2 - edgeSpacing * 1.5);
            confirmButton.Click += (sender, e) =>
            {
                onConfirm?.Invoke();
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(confirmButton);
            dialogWindow.Children.Add(buttonPanel);

            dialogGrid.Children.Add(dialogWindow);
            return dialogGrid;
        }  

        public static (Grid dialogGrid, TextBox titleInput) CreateNewQuiz(Grid parentGrid, string header, string userinputTitle, Action<string> onConfirm)
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

            TextBlock dialogHeader = AddDialogHeader(header, edgeSpacing);
            dialogWindow.Children.Add(dialogHeader);

            var (inputQuizName, titleInput) = AddUserInputTextBox(userinputTitle, dialogWidth - edgeSpacing * 2); 
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
            var closeButton = AddButton("Close", edgeSpacing, HorizontalAlignment.Left, dialogWidth / 2 - edgeSpacing * 1.5);
            closeButton.Click += (sender, e) =>
            {
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(closeButton);

            // Confirm Button
            var confirmButton = AddButton("Confirm", edgeSpacing, HorizontalAlignment.Right, dialogWidth / 2 - edgeSpacing * 1.5);
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

        public static (Grid dialogGrid, ComboBox slideTypeInput) CreateNewQuizSlide(Grid parentGrid, string header, string userinputTitle, List<string> options, string defaultValue, Action<string> onConfirm)
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

            TextBlock dialogHeader = AddDialogHeader(header, edgeSpacing);
            dialogWindow.Children.Add(dialogHeader);

            var (inputQuizName, slideTypeInput) = AddUserInputDropDown(userinputTitle, options, defaultValue, dialogWidth - edgeSpacing * 2); 
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
            var closeButton = AddButton("Close", edgeSpacing, HorizontalAlignment.Left, dialogWidth / 2 - edgeSpacing * 1.5);
            closeButton.Click += (sender, e) =>
            {
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(closeButton);

            // Confirm Button
            var confirmButton = AddButton("Confirm", edgeSpacing, HorizontalAlignment.Right, dialogWidth / 2 - edgeSpacing * 1.5);
            confirmButton.Click += (sender, e) =>
            {
                onConfirm?.Invoke(slideTypeInput?.SelectedValue?.ToString()!);
                parentGrid.Children.Remove(dialogGrid);
            };
            buttonPanel.Children.Add(confirmButton);
            dialogWindow.Children.Add(buttonPanel);

            dialogGrid.Children.Add(dialogWindow);

            return (dialogGrid, slideTypeInput);
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

        private static Grid AddDynamicMessage(string text, int height, int width)
        {
            Grid messageOutline = new Grid
            {
                Height = height,
                Width = width,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Avalonia.Thickness(0,0,0,20)
            };

            TextBlock message = new TextBlock
            {
                Text = text,
                FontSize = 25,
                Classes = { "neon-text" },
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Width = width,
                Foreground = new SolidColorBrush(Color.Parse("#00FFFF"))
            };

            messageOutline.Children.Add(message);
            return messageOutline;
        }

        private static (Grid container, TextBox input) AddUserInputTextBox(string title, double width)
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

        private static (StackPanel container, ComboBox input) AddUserInputDropDown(string title, List<string> options, string defaultValue, double width)
        {
            StackPanel userInput = new StackPanel
            {
                Height = 80,
                Width = width,
                Orientation = Orientation.Horizontal
            };

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

            (Border DropDownElement, ComboBox DropDownReference) = DropdownElement.Create(options , defaultValue, 50); 
            DropDownElement.Width = width * 2/3;
            userInput.Children.Add(DropDownElement);

            return (userInput, DropDownReference);
        }

        private static Button AddButton(string content, int edgeMargin, HorizontalAlignment horizontalAlignment, double width)
        {
            Button button = new Button
            {
                Content = content,
                Classes = { "neon-text-button" },
                HorizontalAlignment = horizontalAlignment,
                Width = width,
                Height = 70,
                Foreground= new SolidColorBrush(Color.Parse("#8C52FF"))
            };

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    button.Margin = new Thickness(edgeMargin,0,0,0);
                    break;
                case HorizontalAlignment.Right:
                    button.Margin = new Thickness(0,0,edgeMargin,0);
                        break;
            }
            return button;
        }

        private static Button? _selectedButton;
        private static Grid AddImageSelection(List<string> imagePaths)
        {
            Grid root = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,*"),
                Margin = new Thickness(50, 100, 50, 150),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            UniformGrid imageGrid = new UniformGrid
            {
                Columns = 6,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            foreach (var path in imagePaths)
            {
                if (!File.Exists(path))
                    continue;

                var bitmap = QuizDataHandler.GetCachedBitmap(path);

                Button imageButton = new Button
                {
                    Name = path,
                    Classes = {"neon-image-button"},
                    Width = 160,
                    Height = 160,
                    Margin = new Thickness(8),
                    Padding = new Thickness(0),
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                };
                if(path == ElementHander.currentSlideImagePath) imageButton.BorderBrush = Brushes.White;

                Image image = new Image
                {
                    Source = bitmap
                };

                imageButton.Content = image;

                imageButton.Click += (_, __) =>
                {
                    foreach(Button element in imageGrid.Children) element.BorderBrush = new SolidColorBrush(Color.Parse("#8C52FF"));

                    // select this button
                    imageButton.BorderBrush = Brushes.White;
                    _selectedButton = imageButton;

                    ElementHander.currentSlideImagePath = path;
                };

                imageGrid.Children.Add(imageButton);
            }

            scrollViewer.Content = imageGrid;
            Grid.SetRow(scrollViewer, 1);
            root.Children.Add(scrollViewer);

            return root;
        }
    
    }
}