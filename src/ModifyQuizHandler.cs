using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using static QuizSlide;

namespace DesktopApp;

public class ModifyQuizHandler
{
    public List<QuizSlide>? slides;
    public QuizSlide? currentSelectedSlide;
    public string? currentSelectedSlideTypeIndex;
    private readonly Dictionary<int, Button> overviewButtons = new();
    private  Grid? quizPageGrid;
    private CurrentSlideData? returnData;


    public ModifyQuizHandler() {}

    public void SetQuizPageGrid(Grid grid)
    {
        quizPageGrid = grid;
    }

    public ScrollViewer InitQuizOverview()
    {
        int textIndex = 1;
        int questionIndex = 1;

        StackPanel quizOverviewPanel = new StackPanel
        {
            Width = 172,
            Orientation = Avalonia.Layout.Orientation.Vertical
        };

        if(slides != null)
        {
            foreach (QuizSlide slide in slides)
            {
                switch (slide.Type)
                {
                    case var t when t == SlideTypes.MultipleChoiceQuestion.ToString():
                        quizOverviewPanel.Children.Add(CreateQuizOverviewButton(slide.Id, $"Q{questionIndex}", questionIndex));
                        questionIndex++;
                        break;

                    case var t when t == SlideTypes.OpenQuestion.ToString():
                        quizOverviewPanel.Children.Add(CreateQuizOverviewButton(slide.Id, $"Q{questionIndex}", questionIndex));
                        questionIndex++;
                        break;

                    case var t when t == SlideTypes.Text.ToString():
                        quizOverviewPanel.Children.Add(CreateQuizOverviewButton(slide.Id, $"T{textIndex}", textIndex));
                        textIndex++;
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("No questions sorted, slides is null");
        }

        ScrollViewer quizOverview = new ScrollViewer
        {
            VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
            Content = quizOverviewPanel
        };

        OpenSlideById(0);

        return quizOverview;
    }

    public void WriteNewQuestionData()
    {
        if (returnData != null && ElementHander.currentOpenQuizTitle != null)
        {
            QuizSlide slide;
            if (returnData.Type == SlideTypes.Text.ToString())
            {
                int headerFontSize =
                    int.TryParse(returnData.Header?.FontSize?.SelectedValue?.ToString(), out var h)
                        ? h
                        : returnData.Header?.Text?.FontSize is > 0
                            ? (int)returnData.Header.Text.FontSize
                            : ElementHander.textSizes.headerTextSize;

                int subTextFontSize =
                    int.TryParse(returnData.SubText?.FontSize?.SelectedValue?.ToString(), out var s)
                        ? s
                        : returnData.SubText?.Text?.FontSize is > 0
                            ? (int)returnData.SubText.Text.FontSize
                            : ElementHander.textSizes.subtextTextSize;

                slide = new QuizSlide
                {
                    Id = returnData.Id,
                    Type = returnData.Type,
                    Header = new QuizSlideText
                    {
                        Text = returnData.Header?.Text?.Text ?? "",
                        FontSize = headerFontSize
                    },
                    SubText = new QuizSlideText
                    {
                        Text = returnData.SubText?.Text?.Text ?? "",
                        FontSize = subTextFontSize
                    },
                    BgImagePath = returnData.BgImagePath,
                    Category = returnData.Category,
                    ImagePath = returnData.ImagePath,
                    AudioPath = returnData.AudioPath
                };
            }
            else
            {
                List<string> newAnswers = [];
                foreach (TextBox awnser in returnData.Answers ?? []) newAnswers.Add(awnser.Text ?? "");
                slide = new QuizSlide
                {
                    Id = returnData.Id,
                    Type = returnData.Type,
                    Question = returnData.Question?.Text,
                    Answers = newAnswers,
                    CorrectAnswer = returnData.GetCurrentCorrectAnswer(),
                    Time = int.Parse(returnData.Time?.SelectedValue?.ToString() ?? ""),
                    BgImagePath = returnData.BgImagePath,
                    Category = returnData.Category,
                    ImagePath = returnData.ImagePath,
                    AudioPath = returnData.AudioPath
                };
            }
            QuizDataHandler.UpdateSlide(ElementHander.currentOpenQuizTitle, slide);
            slides = QuizDataHandler.GetAllQuizSlides(ElementHander.currentOpenQuizTitle);
        }
    }

    public void OpenSlideById(int slideId)
    {
        if (slides == null || slides.Count == 0)
            return;

        int questionIndex = 1;
        int textIndex = 1;

        foreach (var slide in slides)
        {
            if (slide.Id != slideId)
            {
                // Keep track of index for labeling
                switch (slide.Type)
                {
                    case var t when t == SlideTypes.MultipleChoiceQuestion.ToString():
                        questionIndex++;
                        break;
                    case var t when t == SlideTypes.OpenQuestion.ToString():
                        questionIndex++;
                        break;
                    case var t when t == SlideTypes.Text.ToString():
                        textIndex++;
                        break;
                }
                continue;
            }

            int slideTypeIndex = slide.Type == SlideTypes.Text.ToString() ? textIndex : questionIndex;

            SelectSlide(slide.Id, slideTypeIndex);
            currentSelectedSlide = slide;
            currentSelectedSlideTypeIndex = slide.Type == SlideTypes.Text.ToString() ? $"T{slideTypeIndex}" : $"Q{slideTypeIndex}";
            return;
        }
    }


    private Button CreateQuizOverviewButton(int slideId, string content, int slideTypeIndex)
    {
        var button = new Button
        {
            Content = content,
            Classes = { "neon-text-button" },
            Margin = new Thickness(10,10,10,0),
            Height = 80,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            Foreground = new SolidColorBrush(Color.Parse("#8C52FF")),
            BorderBrush = new SolidColorBrush(Color.Parse("#00FFFF")),
        };

        button.Click += (_, _) => HandleSlideClick(slideId, slideTypeIndex);

        overviewButtons[slideId] = button;
        return button;
    }

    private void HandleSlideClick(int slideId, int slideTypeIndex)
    {
        // Save previous slide
        WriteNewQuestionData();
        
        // Show new slide
        SelectSlide(slideId, slideTypeIndex);
    }

    private void SelectSlide(int slideId, int slideTypeIndex)
    {
        if (slides == null)
            return;

        foreach (var slide in slides)
        {
            if (slide.Id != slideId)
                continue;

            currentSelectedSlide = slide;

            Grid slideToShow;

            switch (slide.Type)
            {
                case var t when t == SlideTypes.MultipleChoiceQuestion.ToString():
                    (slideToShow, returnData) = ModifySlideElement.CreateMultipleChoiceQuestionSlide(slide, slideTypeIndex);
                    currentSelectedSlideTypeIndex = $"Q{slideTypeIndex}";
                    break;

                case var t when t == SlideTypes.OpenQuestion.ToString():
                    (slideToShow, returnData) = ModifySlideElement.CreateOpenQuestionSlide(slide, slideTypeIndex);
                    currentSelectedSlideTypeIndex = $"Q{slideTypeIndex}";
                    break;

                default:
                    (slideToShow, returnData) = ModifySlideElement.CreateTextSlide(slide);
                    currentSelectedSlideTypeIndex = $"T{slideTypeIndex}";
                    break;
            }

            quizPageGrid?.Children.Clear();
            quizPageGrid?.Children.Add(slideToShow);

            UpdateSelectedButtonBorder();
            return;
        }
    }


    private void UpdateSelectedButtonBorder()
    {
        foreach (var kvp in overviewButtons)
        {
            kvp.Value.BorderBrush =
                kvp.Key == currentSelectedSlide?.Id
                    ? new SolidColorBrush(Colors.White)
                    : new SolidColorBrush(Color.Parse("#00FFFF"));
        }
    }
}