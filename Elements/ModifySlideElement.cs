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


        //------------------------------------------------------------
        //MUTIPLE CHOICE QUESTION
        //------------------------------------------------------------
        public static (Grid MultipleChoiceQuestionSlide, CurrentSlideData returnData) CreateMultipleChoiceQuestionSlide(QuizSlide slide, int questionNumber)
        {
            ElementHander.currentTime = slide.Time ?? 0;
            ElementHander.currentCorrectAnswer = slide.CorrectAnswer;
            if(slide.Answers != null) ElementHander.currentAmountOfAnswers = slide.Answers.Count.ToString();

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
            slideBorder.Child = slideGrid;

            (Border QuizQuestion, TextBox questionText) = CreateSlideHeaderElement.Create(questionNumber, slide.Question ?? "", 200, VerticalAlignment.Top, false, "#FFFFFF", "#8C52FF");
            slideGrid.Children.Add(QuizQuestion);

            
            if (slide.Answers != null)
            {
                (Grid multipleChoiceOptions, ElementHander.currentAwnserOptions) =
                    MultipleChoiceOptionsElement.Create(
                        slide.Answers,
                        MultipleChoiceOptionColors,
                        ElementHander.currentAmountOfAnswers ?? "",
                        400
                    );

                ElementHander.currentMultipleChoiceGrid = multipleChoiceOptions;
                slideGrid.Children.Add(multipleChoiceOptions);
            }

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

            ElementHander.currentData = new CurrentSlideData
            {
                Id = slide.Id,
                Type = slide.Type,
                Question = questionText,
                Answers = ElementHander.currentAwnserOptions,
                CorrectAnswerComboBox = new ComboBox { SelectedValue = slide.CorrectAnswer },
                Time = new ComboBox { SelectedValue = slide.Time?.ToString() },
                BgImagePath = "avares://DesktopApp/Assets/Backgrounds/BricksDesktop.png",
                Category = "",
                ImagePath = "",
                AudioPath = ""
            };
            optionsButton.Click += (_, _) =>
            {
                if (ElementHander.currentData != null) {
                    ElementHander.currentData.Time = ElementHander.OpenSlidePanelClick(slide);
                    ElementHander.currentData.CorrectAnswerComboBox = ElementHander.currentCorrectAnswerDropDown;
                }
            };
            
            
            MainGrid.Children.Add(slideBorder);

            return (MainGrid, ElementHander.currentData);
        }


        //------------------------------------------------------------
        //OPEN QUESTION
        //------------------------------------------------------------
        public static (Grid MultipleChoiceQuestionSlide, CurrentSlideData returnData) CreateOpenQuestionSlide(QuizSlide slide, int questionNumber)
        {
            ElementHander.currentTime = slide.Time ?? 0;
            ElementHander.currentCorrectAnswer = slide.CorrectAnswer;

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
            slideBorder.Child = slideGrid;

            (Border QuizQuestion, TextBox questionText) = CreateSlideHeaderElement.Create(questionNumber, slide.Question ?? "", 500, VerticalAlignment.Center, true, "#FFFFFF", "#8C52FF");
            slideGrid.Children.Add(QuizQuestion);

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

            ElementHander.currentData = new CurrentSlideData
            {
                Id = slide.Id,
                Type = slide.Type,
                Question = questionText,
                Answers = [],
                CorrectAnswer = new TextBox{ Text =  ElementHander.currentCorrectAnswer},
                Time = new ComboBox { SelectedValue = slide.Time?.ToString() },
                BgImagePath = "avares://DesktopApp/Assets/Backgrounds/BricksDesktop.png",
                Category = "",
                ImagePath = "",
                AudioPath = ""
            };
            optionsButton.Click += (_, _) =>
            {
                if (ElementHander.currentData != null) {
                    ElementHander.currentData.Time = ElementHander.OpenSlidePanelClick(slide);
                    ElementHander.currentData.CorrectAnswer = ElementHander.currentCorrectAnswerTextBox;
                }
            };

            MainGrid.Children.Add(slideBorder);

            return (MainGrid, ElementHander.currentData);
        }

        //------------------------------------------------------------
        //TEXT
        //------------------------------------------------------------
        public static (Grid MultipleChoiceQuestionSlide, CurrentSlideData returnData) CreateTextSlide(QuizSlide slide)
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
            slideBorder.Child = slideGrid;

            (Border SlideHeader, TextBox headerText) = CreateSlideHeaderElement.Create(0, slide.Header ?? "", 200, VerticalAlignment.Top, false, "#FFFFFF", "#8C52FF");
            slideGrid.Children.Add(SlideHeader);


            ElementHander.currentData = new CurrentSlideData
            {
                Id = slide.Id,
                Type = slide.Type,
                Header = headerText,
                SubText = new TextBox(),
                BgImagePath = "avares://DesktopApp/Assets/Backgrounds/BricksDesktop.png",
                Category = "",
                ImagePath = "",
                AudioPath = ""
            };


            MainGrid.Children.Add(slideBorder);

            return (MainGrid, ElementHander.currentData);
        }

        
    }
}

