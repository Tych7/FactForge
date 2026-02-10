using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using static QuizSlide;

namespace DesktopApp;

public class ModifyQuizHandler
{
    // --- Singleton instance --- 
    private static readonly Lazy<ModifyQuizHandler> _instance = new(() => new ModifyQuizHandler()); 
    public static ModifyQuizHandler Instance => _instance.Value;

    public List<QuizSlide>? slides;
    public event Action? QuizOverviewNeedsRefresh;
    public QuizSlide? currentSelectedSlide;
    public string? currentSelectedSlideTypeIndex;
    public readonly Dictionary<int, Button> overviewButtons = new();
    public bool FirstInit = false;

    private ScrollViewer? _quizOverview;
    private  Grid? _quizPageGrid;
    private CurrentSlideData? _returnData;

    public ModifyQuizHandler() {}

    public void SetQuizPageGrid(Grid grid)
    {
        _quizPageGrid = grid;
    }

    public ScrollViewer InitQuizOverview()
    {
        if (FirstInit == false)
        {
            FirstInit = true; 
            OpenSlideById(0);
        }

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
                    case var t when t == SlideTypes.MultipleChoiceQuestion:
                        quizOverviewPanel.Children.Add(CreateSlideOverviewElements.CreateSlideElement(slide, $"Q{questionIndex}", questionIndex));
                        questionIndex++;
                        break;

                    case var t when t == SlideTypes.OpenQuestion:
                        quizOverviewPanel.Children.Add(CreateSlideOverviewElements.CreateSlideElement(slide, $"Q{questionIndex}", questionIndex));
                        questionIndex++;
                        break;

                    case var t when t == SlideTypes.Text:
                        quizOverviewPanel.Children.Add(CreateSlideOverviewElements.CreateSlideElement(slide, $"T{textIndex}", textIndex));
                        textIndex++;
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("No questions sorted, slides is null");
        }

        UpdateSelectedButtonBorder();   

        _quizOverview = new ScrollViewer
        {
            VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
            Content = quizOverviewPanel
        };


        return _quizOverview;
    }

    public void WriteNewQuestionData()
    {
        if (_returnData != null && ElementHander.currentOpenQuizTitle != null)
        {
            QuizSlide slide;
            if (_returnData.Type == SlideTypes.Text)
            {
                int headerFontSize =
                    int.TryParse(_returnData.Header?.FontSize?.SelectedValue?.ToString(), out var h)
                        ? h
                        : _returnData.Header?.Text?.FontSize is > 0
                            ? (int)_returnData.Header.Text.FontSize
                            : ElementHander.textSizes.headerTextSize;

                int subTextFontSize =
                    int.TryParse(_returnData.SubText?.FontSize?.SelectedValue?.ToString(), out var s)
                        ? s
                        : _returnData.SubText?.Text?.FontSize is > 0
                            ? (int)_returnData.SubText.Text.FontSize
                            : ElementHander.textSizes.subtextTextSize;

                slide = new QuizSlide
                {
                    Id = _returnData.Id,
                    Type = _returnData.Type,
                    Header = new QuizSlideText
                    {
                        Text = _returnData.Header?.Text?.Text ?? "",
                        FontSize = headerFontSize
                    },
                    SubText = new QuizSlideText
                    {
                        Text = _returnData.SubText?.Text?.Text ?? "",
                        FontSize = subTextFontSize
                    },
                    BgImagePath = _returnData.BgImagePath,
                    Category = _returnData.Category,
                    ImagePath = ElementHander.currentSlideImagePath,
                    AudioPath = _returnData.AudioPath
                };
            }
            else
            {
                List<string> newAnswers = [];
                foreach (TextBox awnser in _returnData.Answers ?? []) newAnswers.Add(awnser.Text ?? "");
                slide = new QuizSlide
                {
                    Id = _returnData.Id,
                    Type = _returnData.Type,
                    Question = _returnData.Question?.Text,
                    Answers = newAnswers,
                    CorrectAnswer = _returnData.GetCurrentCorrectAnswer(),
                    Time = int.Parse(_returnData.Time?.SelectedValue?.ToString() ?? ""),
                    BgImagePath = _returnData.BgImagePath,
                    Category = _returnData.Category,
                    ImagePath = ElementHander.currentSlideImagePath,
                    AudioPath = _returnData.AudioPath
                };
            }
            QuizDataHandler.UpdateSlide(ElementHander.currentOpenQuizTitle, slide);
            slides = QuizDataHandler.GetQuizData(ElementHander.currentOpenQuizTitle).Quiz;
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
                    case var t when t == SlideTypes.MultipleChoiceQuestion:
                        questionIndex++;
                        break;
                    case var t when t == SlideTypes.OpenQuestion:
                        questionIndex++;
                        break;
                    case var t when t == SlideTypes.Text:
                        textIndex++;
                        break;
                }
                continue;
            }

            int slideTypeIndex = slide.Type == SlideTypes.Text ? textIndex : questionIndex;

            SelectSlide(slide.Id, slideTypeIndex);
            currentSelectedSlide = slide;
            currentSelectedSlideTypeIndex = slide.Type == SlideTypes.Text ? $"T{slideTypeIndex}" : $"Q{slideTypeIndex}";
            return;
        }
    }

    public void SelectSlide(int slideId, int slideTypeIndex)
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
                case var t when t == SlideTypes.MultipleChoiceQuestion:
                    (slideToShow, _returnData) = ModifySlideElement.CreateMultipleChoiceQuestionSlide(slide, slideTypeIndex);
                    currentSelectedSlideTypeIndex = $"Q{slideTypeIndex}";
                    break;

                case var t when t == SlideTypes.OpenQuestion:
                    (slideToShow, _returnData) = ModifySlideElement.CreateOpenQuestionSlide(slide, slideTypeIndex);
                    currentSelectedSlideTypeIndex = $"Q{slideTypeIndex}";
                    break;

                default:
                    (slideToShow, _returnData) = ModifySlideElement.CreateTextSlide(slide);
                    currentSelectedSlideTypeIndex = $"T{slideTypeIndex}";
                    break;
            }

            _quizPageGrid?.Children.Clear();
            _quizPageGrid?.Children.Add(slideToShow);

            UpdateSelectedButtonBorder();
            QuizOverviewNeedsRefresh?.Invoke();
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