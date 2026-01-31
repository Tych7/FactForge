using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using static QuizSlide;

namespace DesktopApp;

public partial class CreateQuizScreen : UserControl
{
    private TextBox? _quizTitleHeader;

    public CreateQuizScreen(string quizTitle)
    {
        InitializeComponent();
        ModifyQuizHandler.Instance.FirstInit = false;

        ModifyQuizHandler.Instance.SetQuizPageGrid(Quizpage);
        ModifyQuizHandler.Instance.QuizOverviewNeedsRefresh += RefreshQuizOverviewCallback;
        CreateSlideOverviewElements.InsertSlide += InsertSlideClick;

        FetchAndShowSlides(quizTitle);
        ElementHander.currentOpenQuizTitle = quizTitle;

        CreateAndSetQuizTitleHeader(quizTitle);
    }

    private void CreateAndSetQuizTitleHeader(string QuizTitle)
    {
        _quizTitleHeader = new TextBox
        {
            Classes = {"neon-input"},
            Text = QuizTitle,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
        };
        ElementHander.AutoFitTextBox(_quizTitleHeader, false);

        QuizTitleHeader.Children.Add(_quizTitleHeader);
    }

    private void FetchAndShowSlides(string quizTitle)
    {
        ModifyQuizHandler.Instance.slides = QuizDataHandler.GetQuizData(quizTitle).Quiz;
        QuizOverview.Child = ModifyQuizHandler.Instance.InitQuizOverview();
    }

    private void RefreshQuizOverviewCallback()
    {
        QuizOverview.Child = ModifyQuizHandler.Instance.InitQuizOverview();
    }

    private void InsertSlideClick(int InsertId)
    {
        List<string> TypeOptions = [];
        foreach (SlideTypes type in Enum.GetValues(typeof(SlideTypes))) TypeOptions.Add(type.ToString());
        (Grid dialogGrid, ComboBox slideTypeInput) = Dialog.CreateNewQuizSlide(MainGrid, "Add new slide", "Type", TypeOptions, SlideTypes.Text.ToString(), (selectedType) =>
        {
            foreach (SlideTypes type in Enum.GetValues(typeof(SlideTypes)))
            {
                if(type.ToString() == selectedType)
                {
                    if (ModifyQuizHandler.Instance.currentSelectedSlide != null)
                    {
                        bool result = QuizDataHandler.InsertSlideAtIndex(ElementHander.currentOpenQuizTitle ?? "", type, InsertId);
                        FetchAndShowSlides(ElementHander.currentOpenQuizTitle ?? "");
                        ModifyQuizHandler.Instance.OpenSlideById(InsertId);
                    }
                }
            }
        });
        MainGrid.Children.Add(dialogGrid);
    }


    private void NewSlideClick(object? sender, RoutedEventArgs e)
    {
        List<string> TypeOptions = [];
        foreach (SlideTypes type in Enum.GetValues(typeof(SlideTypes))) TypeOptions.Add(type.ToString());
        (Grid dialogGrid, ComboBox slideTypeInput) = Dialog.CreateNewQuizSlide(MainGrid, "Add new slide", "Type", TypeOptions, SlideTypes.Text.ToString(), (selectedType) =>
        {
            foreach (SlideTypes type in Enum.GetValues(typeof(SlideTypes)))
            {
                if(type.ToString() == selectedType)
                {
                    (bool result, int slideId) = QuizDataHandler.CreateNewSlide(ElementHander.currentOpenQuizTitle ?? "", type);
                    FetchAndShowSlides(ElementHander.currentOpenQuizTitle ?? "");
                    ModifyQuizHandler.Instance.OpenSlideById(slideId);
                }
            }
        });
        MainGrid.Children.Add(dialogGrid);
    }

    private void DeleteSlideClick(object? sender, RoutedEventArgs e)
    {
        if(ModifyQuizHandler.Instance.currentSelectedSlide != null)
        {
            var dialog = Dialog.AreYouSure( MainGrid, $"When you press the confim button slide '{ModifyQuizHandler.Instance.currentSelectedSlideTypeIndex}' will be deleted.", () =>
            {
                bool deleteResult = QuizDataHandler.DeleteSlide(ElementHander.currentOpenQuizTitle ?? "", ModifyQuizHandler.Instance.currentSelectedSlide.Id);
                if (deleteResult)
                {
                    bool reassignResult = QuizDataHandler.ReassignSlideIds(ElementHander.currentOpenQuizTitle ?? ""); 
                    if(reassignResult && ElementHander.currentOpenQuizTitle != null) FetchAndShowSlides(ElementHander.currentOpenQuizTitle);

                    int slideIdToSelect = 0;
                    if(ModifyQuizHandler.Instance.currentSelectedSlide.Id == ModifyQuizHandler.Instance.slides?.Count)
                    {
                        slideIdToSelect = ModifyQuizHandler.Instance.currentSelectedSlide.Id - 1;
                    }
                    else slideIdToSelect = ModifyQuizHandler.Instance.currentSelectedSlide.Id;
                    ModifyQuizHandler.Instance.OpenSlideById(slideIdToSelect);
                } 
            });
            MainGrid.Children.Add(dialog); 
        }
    }


    private void BackClick(object? sender, RoutedEventArgs e)
    {
        ModifyQuizHandler.Instance.WriteNewQuestionData();

        if(ElementHander.currentOpenQuizTitle != null && _quizTitleHeader != null)
        {
            if(ElementHander.currentOpenQuizTitle != _quizTitleHeader.Text)
            {
                var currentQuizData = QuizDataHandler.GetQuizData(ElementHander.currentOpenQuizTitle);
                currentQuizData.Title = _quizTitleHeader.Text;
                QuizDataHandler.ReplaceQuizData(ElementHander.currentOpenQuizTitle, currentQuizData);
            }
        }
        
        
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new QuizLibraryScreen();
            }
        }
    }


}
