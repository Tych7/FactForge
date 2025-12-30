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
    public CreateQuizScreen(string quizTitle)
    {
        InitializeComponent();

        ModifyQuizHandler.Instance.SetQuizPageGrid(Quizpage);
        ModifyQuizHandler.Instance.QuizOverviewNeedsRefresh += RefreshQuizOverview;
        CreateSlideOverviewElements.InsertSlide += InsertSlideClick;

        FetchAndShowSlides(quizTitle);
        ElementHander.currentOpenQuizTitle = quizTitle;
    }

    private void FetchAndShowSlides(string quizTitle)
    {
        ModifyQuizHandler.Instance.slides = QuizDataHandler.GetAllQuizSlides(quizTitle);
        QuizOverview.Child = ModifyQuizHandler.Instance.InitQuizOverview();
    }

    private void RefreshQuizOverview()
    {
        QuizOverview.Child = ModifyQuizHandler.Instance.InitQuizOverview();
    }

    private void InsertSlideClick()
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
                        bool result = QuizDataHandler.InsertSlideAtIndex(ElementHander.currentOpenQuizTitle ?? "", type, ModifyQuizHandler.Instance.currentSelectedSlide.Id + 1);
                        FetchAndShowSlides(ElementHander.currentOpenQuizTitle ?? "");
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
                    ModifyQuizHandler.Instance.OpenSlideById(ModifyQuizHandler.Instance.currentSelectedSlide.Id);
                } 
            });
            MainGrid.Children.Add(dialog); 
        }
    }


    private void BackClick(object? sender, RoutedEventArgs e)
    {
        ModifyQuizHandler.Instance.WriteNewQuestionData();
        
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new QuizLibraryScreen();
            }
        }
    }


}
