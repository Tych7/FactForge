
using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;
using Avalonia.Media;
using MsBox.Avalonia.ViewModels;
using Tmds.DBus.Protocol;

namespace DesktopApp
{
    public static class CreateSlideOverviewElements
    {   
        public static event Action? InsertSlide; 
        public static Button CreateSlideElement(QuizSlide slide, string content, int slideTypeIndex)
        {
            var contentGrid = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Orientation = Orientation.Horizontal 
            };
            
            Border slideElementIcon = CreateSlideElementIcon("question_regular", 40,40);
            contentGrid.Children.Add(slideElementIcon);

            var slideElementText = new TextBlock
            {
                Text = content,
                Classes = {"neon-text"},
                FontSize = 35,
                VerticalAlignment = VerticalAlignment.Center,
            };
            contentGrid.Children.Add(slideElementText);

            var button = new Button
            {
                Content = contentGrid,
                Margin = new Thickness(10,10,10,0),
                Height = 80,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Foreground = new SolidColorBrush(Color.Parse("#8C52FF")),
                Classes = {"neon-empty-button"}
            };

            button.Click += (_, _) =>
            {
                // Save previous slide
                ModifyQuizHandler.Instance.WriteNewQuestionData();
                
                // Show new slide
                ModifyQuizHandler.Instance.SelectSlide(slide.Id, slideTypeIndex);

            };

            ModifyQuizHandler.Instance.overviewButtons[slide.Id] = button;
            return button;
        }

        public static Button CreateInsertElement()
        {
            var addIcon = new PathIcon
            {
                Data = AppHandler.GetIcon("add_regular"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 20,
                Width = 20
            };

            Button button = new Button
            {
                Height = 50,
                Width = 50,
                Classes = {"neon-icon-button"},
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BorderThickness = new Thickness(3),
                CornerRadius = new CornerRadius(30),
                Content = new Border { Child = addIcon },                
                Margin = new Thickness(0,10,0,0)
            };

            button.Click += (_, _) =>
            {
                Console.WriteLine($"Inserting slide after slide: {ModifyQuizHandler.Instance?.currentSelectedSlide?.Id}");
                if (InsertSlide != null) InsertSlide.Invoke();
            };

            return button;
        }

        private static Border CreateSlideElementIcon(string pathIconName, int height, int width)
        {   
            var slideIcon = new PathIcon
            {
                Data = AppHandler.GetIcon(pathIconName),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = height - 10,
                Width = width - 10,
                Foreground = new SolidColorBrush(Color.Parse("#8C52FF")),
            };

            var iconBorder = new Border
            {
                Height = height,
                Width = height,
                CornerRadius = new CornerRadius(height / 2),
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
            };
            iconBorder.Child = slideIcon;

            return iconBorder;
        }
    }
}