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

        OpenFirstSlide();

        return quizOverview;
    }

    public void WriteNewQuestionData()
    {
        if (returnData != null && ElementHander.currentOpenQuizTitle != null)
        {
            QuizSlide slide;
            if (returnData.Type == SlideTypes.Text.ToString())
            {
                slide = new QuizSlide
                {
                    Id = returnData.Id,
                    Type = returnData.Type,
                    Header = returnData.Header?.Text,
                    SubText = returnData.SubText?.Text,
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

    private void OpenFirstSlide()
    {
        if (slides == null || slides.Count == 0)
            return;

        int questionIndex = 1;
        int textIndex = 1;

        foreach (QuizSlide slide in slides)
        {
            switch (slide.Type)
            {
                case var t when t == SlideTypes.MultipleChoiceQuestion.ToString():
                    SetCurrentSelectedSlide(slide.Id, questionIndex);
                    currentSelectedSlide = slide;
                    currentSelectedSlideTypeIndex = $"Q{questionIndex}";
                    return;

                case var t when t == SlideTypes.OpenQuestion.ToString():
                    SetCurrentSelectedSlide(slide.Id, questionIndex);
                    currentSelectedSlide = slide;
                    currentSelectedSlideTypeIndex = $"Q{questionIndex}";
                    return;

                case var t when t == SlideTypes.Text.ToString():
                    SetCurrentSelectedSlide(slide.Id, textIndex);
                    currentSelectedSlide = slide;
                    currentSelectedSlideTypeIndex = $"T{textIndex}";
                    return;
            }
        }

        UpdateSelectedButtonBorder();
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

        button.Click += (_, _) => SetCurrentSelectedSlide(slideId, slideTypeIndex);

        overviewButtons[slideId] = button;
        return button;
    }

    private void SetCurrentSelectedSlide(int slideId, int slideTypeIndex)
    {
        if (slides == null)
            return;

        foreach (var slide in slides)
        {
            if (slide.Id != slideId)
                continue;

            WriteNewQuestionData();

            currentSelectedSlide = slide;
            UpdateSelectedButtonBorder();

            Grid slideToShow;

            switch (slide.Type)
            {
                case var t when t == SlideTypes.MultipleChoiceQuestion.ToString():
                    (slideToShow, returnData) =
                        ModifySlideElement.CreateMultipleChoiceQuestionSlide(slide, slideTypeIndex);
                        currentSelectedSlideTypeIndex = $"Q{slideTypeIndex}";
                    break;

                case var t when t == SlideTypes.OpenQuestion.ToString():
                    (slideToShow, returnData) =
                        ModifySlideElement.CreateOpenQuestionSlide(slide, slideTypeIndex);
                        currentSelectedSlideTypeIndex = $"Q{slideTypeIndex}";
                    break;

                default:
                    (slideToShow, returnData) =
                        ModifySlideElement.CreateTextSlide(slide);
                        currentSelectedSlideTypeIndex = $"T{slideTypeIndex}";
                    break;
            }

            quizPageGrid?.Children.Clear();
            quizPageGrid?.Children.Add(slideToShow);
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