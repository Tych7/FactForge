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
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using static CurrentSlideData;
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

            (Border QuizQuestion, TextBox questionText) = QuizSlideElements.CreateQuestion(questionNumber, slide.Question ?? "", 200, VerticalAlignment.Top, false, "#FFFFFF", "#8C52FF");
            slideGrid.Children.Add(QuizQuestion);

            
            if (slide.Answers != null)
            {
                (Grid multipleChoiceOptions, ElementHander.currentAwnserOptions) = MultipleChoiceOptionsElement.Create( slide.Answers, MultipleChoiceOptionColors, slide.CorrectAnswer ?? "", ElementHander.currentAmountOfAnswers ?? "", 400 );
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

            (Border QuizQuestion, TextBox questionText) = QuizSlideElements.CreateQuestion(questionNumber, slide.Question ?? "", 500, VerticalAlignment.Center, true, "#FFFFFF", "#8C52FF");
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
            if(slide.Header != null) ElementHander.textSizes.headerTextSize = slide.Header.FontSize;
            if(slide.SubText != null) ElementHander.textSizes.subtextTextSize = slide.SubText.FontSize;

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

            StackPanel textPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            TextBox headerText = QuizSlideElements.CreateHeader(slide.Header?.Text ?? "", ElementHander.textSizes.headerTextSize, false);
            ElementHander.currentHeaderText = headerText;
            textPanel.Children.Add(headerText);

            TextBox subText = QuizSlideElements.CreateSubText(slide.SubText?.Text ?? "", ElementHander.textSizes.subtextTextSize, true);
            ElementHander.currentSubtextText = subText;
            textPanel.Children.Add(subText);

            slideGrid.Children.Add(textPanel);

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
                Header = new CurrentSlideText { Text = headerText, FontSize = new ComboBox { SelectedValue = slide.Header?.FontSize } },                
                SubText = new CurrentSlideText { Text = subText, FontSize = new ComboBox { SelectedValue = slide.SubText?.FontSize } },                
                BgImagePath = "avares://DesktopApp/Assets/Backgrounds/BricksDesktop.png",
                Category = "",
                ImagePath = "",
                AudioPath = ""
            };

            optionsButton.Click += (_, _) =>
            {
                if (ElementHander.currentData != null)
                {
                    ElementHander.OpenSlidePanelClick(slide);
                    ElementHander.currentData.Header.FontSize = ElementHander.currentheaderTextSizeDropDown;             
                    ElementHander.currentData.SubText.FontSize = ElementHander.currentsubTextSizeDropDown;
                } 

            };


            MainGrid.Children.Add(slideBorder);

            return (MainGrid, ElementHander.currentData);
        }

        
    }
}

