using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using static QuizSlide;


namespace DesktopApp
{
    public static class ModifySlideElement
    {
        public static List<string> MultipleChoiceOptionColors = ["#006EFF", "#FF0000", "#00FF00", "#FFFF00"];

        private static CurrentSlideData? currentData;

        private static int selectedQuestionTime;

        private static List<TextBox>? currentAwnserOptions;
        public static string? currentCorrectAnswer;


        private static (ComboBox timeDropDown, ComboBox awnserDropDown) OpenPanelClick(QuizSlide slide)
        {
            StackPanel slideOptions = new StackPanel();

            //TIME OPTIONS
            StackPanel timeOption = new StackPanel{Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(0,20,0,0)};
            TextBlock timeTitle = new TextBlock {Text = "Time: ", Classes = {"neon-text"}, FontSize = 40,  VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left}; timeOption.Children.Add(timeTitle);

            var timeOptions = Enumerable.Range(1, 100).Select(i => i.ToString()).ToList();
            selectedQuestionTime = slide.Time ?? 0;

            (Border timeDropDownElement, ComboBox timeDropDown) = DropdownElement.Create(timeOptions, selectedQuestionTime.ToString() ?? "", 50); timeOption.Children.Add(timeDropDownElement);
            timeDropDownElement.Margin = new Thickness(0,10,0,0);
            timeDropDown.SelectionChanged += (_, __) =>
            {
                if (timeDropDown.SelectedItem is string selected && int.TryParse(selected, out int value))
                {
                    selectedQuestionTime = value;
                }
            };
            slideOptions.Children.Add(timeOption);

            //CORRECT AWNSER OPTIONS
            StackPanel correctAwnserOption = new StackPanel{Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Stretch, Margin = new Thickness(0,20,0,0)};
            ComboBox awnserDropDown = new ComboBox();
            if (slide.Type == SlideTypes.MultipleChoiceQuestion.ToString())
            {
                TextBlock awnserTitle = new TextBlock {Text = "Correct Answer: ", Classes = {"neon-text"}, FontSize = 40,  VerticalAlignment= VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left}; correctAwnserOption.Children.Add(awnserTitle);
                List<string> options = [];
                foreach(var option in currentAwnserOptions ?? []) {options.Add(option.Text ?? "");}
                (Border awnserDropDownElement, awnserDropDown) = DropdownElement.Create(options ?? [], currentCorrectAnswer ?? "", 50); correctAwnserOption.Children.Add(awnserDropDownElement);
                awnserDropDownElement.Margin = new Thickness(0,10,0,0);
                awnserDropDown.SelectionChanged += (_, __) =>
                {
                    if (awnserDropDown.SelectedItem is string selected)
                    {
                        currentCorrectAnswer = selected;
                    }
                };
                slideOptions.Children.Add(correctAwnserOption);
            }

            var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                var optionPanel = SlidingPanelElement.Create(mainWindow.ModalLayerContainer, slideOptions, "OPTIONS", 500);
                if (optionPanel != null)
                {
                    mainWindow.ModalLayerContainer.IsHitTestVisible = true;
                    mainWindow.ModalLayerContainer.Children.Add(optionPanel);
                }
            }

            return (timeDropDown, awnserDropDown);
        }

        public static (Grid MultipleChoiceQuestionSlide, CurrentSlideData returnData) CreateMultipleChoiceQuestionSlide(QuizSlide slide, int questionNumber)
        {
            selectedQuestionTime = slide.Time ?? 0;
            currentCorrectAnswer = slide.CorrectAnswer;

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

            
            if(slide.Answers != null)
            {
                (Grid MultipleChoiceOptions, currentAwnserOptions) = MultipleChoiceOptionsElement.Create(slide.Answers, MultipleChoiceOptionColors, slide.Answers.Count, 400);
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
                Answers = currentAwnserOptions,
                CorrectAnswerComboBox = new ComboBox { SelectedValue = slide.CorrectAnswer?.ToString() },
                Time = new ComboBox { SelectedValue = slide.Time?.ToString() },
                BgImagePath = "",
                Category = "",
                ImagePath = "",
                AudioPath = ""
            };
            optionsButton.Click += (_, _) =>
            {
                if (currentData != null) {
                    (currentData.Time, currentData.CorrectAnswerComboBox) = OpenPanelClick(slide);
                }
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

