using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;


namespace DesktopApp
{
    public static class ModifySlideElement
    {
        public static List<string> MultipleChoiceOptionColors = ["#006EFF", "#FF0000", "#00FF00", "#FFFF00"];

        private static CurrentSlideData? currentData;

        private static int selectedQuestionTime;


        private static ComboBox OpenPanelClick(Grid MainGrid)
        {
            StackPanel slideOptions = new StackPanel();

            
            StackPanel timeOption = new StackPanel{Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(20,20,0,0)};
            TextBlock timeTitle = new TextBlock {Text = "Time: ", Classes = {"neon-text"}, FontSize = 30,  VerticalAlignment= VerticalAlignment.Center}; timeOption.Children.Add(timeTitle);

            var timeOptions = Enumerable.Range(1, 100).Select(i => i.ToString()).ToList();
            (Border dropDownElement, ComboBox dropDown) = DropdownElement.Create(timeOptions, selectedQuestionTime.ToString() ?? "", 300, 50); timeOption.Children.Add(dropDownElement);

            dropDown.SelectionChanged += (_, __) =>
            {
                if (dropDown.SelectedItem is string selected && int.TryParse(selected, out int value))
                {
                    selectedQuestionTime = value;
                }
            };

            slideOptions.Children.Add(timeOption);

            var optionPanel = SlidingPanelElement.Create(MainGrid, slideOptions, "OPTIONS", 500);

            if (optionPanel != null) MainGrid.Children.Add(optionPanel);

            return dropDown;
        }

        public static (Grid MultipleChoiceQuestionSlide, CurrentSlideData returnData) CreateMultipleChoiceQuestionSlide(QuizSlide slide, int questionNumber)
        {
            selectedQuestionTime = slide.Time ?? 0;

            Grid MainGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch  
            };

            var slideBg = new Uri(slide.BgImagePath ?? "");

            Border slideBorder = new Border
            {
                Classes = { "neon-frame" },
                Height = 972,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new ImageBrush { 
                    Source = new Bitmap(AssetLoader.Open(slideBg)),
                    Stretch = Stretch.UniformToFill 
                }
            };

            Grid slideGrid = new Grid
            {
                Margin = new Thickness(20)
            };

            (Border QuizQuestion, TextBox questionText) = CreateQuizQuestionElement.Create(questionNumber, slide.Question ?? "", 200, VerticalAlignment.Top, false, "#FFFFFF", "#8C52FF");
            slideGrid.Children.Add(QuizQuestion);

            List<TextBox> newOptions = [];
            if(slide.Answers != null)
            {
                (Grid MultipleChoiceOptions, newOptions) = MultipleChoiceOptionsElement.Create(slide.Answers, MultipleChoiceOptionColors, slide.Answers.Count, 400);
                slideGrid.Children.Add(MultipleChoiceOptions);
            }
            slideBorder.Child = slideGrid;

            Button optionsButton = new Button
            {
                Height = 80,
                Classes = { "neon-text-button" },
                Content = "OPTIONS",
                Margin = new Thickness(0,20,50,0),
                Foreground = new SolidColorBrush(Color.Parse("#8C52FF")),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                BorderThickness = new Thickness(6,6,6,0),
                CornerRadius = new CornerRadius(10,10,0,0)
            };
            MainGrid.Children.Add(optionsButton);

            currentData = new CurrentSlideData
            {
                Id = slide.Id,
                Type = slide.Type,
                Question = questionText,
                Answers = newOptions,
                CorrectAnswer = new TextBox(),
                BgImagePath = "",
                Category = "",
                ImagePath = "",
                AudioPath = ""
            };
            optionsButton.Click += (_, _) =>
            {
                if (currentData != null) currentData.Time = OpenPanelClick(MainGrid);
            };
            
            MainGrid.Children.Add(slideBorder);

            return (MainGrid, currentData);
        }

        public static Grid CreateOpenQuestionSlide(QuizSlide slide, int questionNumber)
        {
            Grid MainGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch  
            };

            var slideBg = new Uri(slide.BgImagePath ?? "");

            Border slideBorder = new Border
            {
                Classes = { "neon-frame" },
                Height = 972,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new ImageBrush { 
                    Source = new Bitmap(AssetLoader.Open(slideBg)),
                    Stretch = Stretch.UniformToFill 
                }
            };

            Grid slideGrid = new Grid
            {
                Margin = new Thickness(20)
            };

            (Border QuizQuestion, TextBox questionText) = CreateQuizQuestionElement.Create(questionNumber, slide.Question ?? "", 500, VerticalAlignment.Center, true, "#FFFFFF", "#8C52FF");
            slideGrid.Children.Add(QuizQuestion);

            slideBorder.Child = slideGrid;

            MainGrid.Children.Add(slideBorder);

            return MainGrid;
        }

        public static Grid CreateTextSlide(QuizSlide slide)
        {
            Grid MainGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch  
            };

            var slideBg = new Uri(slide.BgImagePath ?? "");

            Border slideBorder = new Border
            {
                Classes = { "neon-frame" },
                Height = 972,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new ImageBrush { 
                    Source = new Bitmap(AssetLoader.Open(slideBg)),
                    Stretch = Stretch.UniformToFill 
                }
            };

            Grid slideGrid = new Grid
            {
                Margin = new Thickness(20)
            };

            TextBox Header = new TextBox
            {
                FontSize = 40,
                Height = 150,
                Classes = { "neon-input" },
                Text = slide.Question,
                VerticalAlignment = VerticalAlignment.Top,
            };
            slideGrid.Children.Add(Header);

            slideBorder.Child = slideGrid;

            MainGrid.Children.Add(slideBorder);

            return MainGrid;
        }

    }
}

