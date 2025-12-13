using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using static QuizSlide;

namespace DesktopApp;

public class ModifyQuizHandler
{
    public List<QuizSlide>? slides;

    public List<QuizSlide>? openQuestions;
    public List<QuizSlide>? multipleChoiseQuestions;
    public List<QuizSlide>? Text;


    public ModifyQuizHandler()
    {

    }

    public ScrollViewer CreateQuizOverview()
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
                        quizOverviewPanel.Children.Add(CreateQuizOverviewButton(slide.Id, $"Q{questionIndex}"));
                        questionIndex++;
                        break;

                    case var t when t == SlideTypes.OpenQuestion.ToString():
                        quizOverviewPanel.Children.Add(CreateQuizOverviewButton(slide.Id, $"Q{questionIndex}"));
                        questionIndex++;
                        break;

                    case var t when t == SlideTypes.Text.ToString():
                        quizOverviewPanel.Children.Add(CreateQuizOverviewButton(slide.Id, $"T{textIndex}"));
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


        return quizOverview;

    }

    private Button CreateQuizOverviewButton(int slideId, string content)
    {
        return new Button
        {
            Name = slideId.ToString(),
            Content = content,
            Classes = {"neon-text-button"},
            Margin = new Avalonia.Thickness(10,10,10,0),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            Height = 80,
            Foreground = new SolidColorBrush(Color.Parse("#8C52FF"))
        };

        
    } 

}