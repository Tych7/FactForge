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
    private ModifyQuizHandler? modifyQuizHandler;

    public CreateQuizScreen(string quizTitle)
    {
        InitializeComponent();

        modifyQuizHandler = new ModifyQuizHandler();
        modifyQuizHandler.SetQuizPageGrid(Quizpage);

        LoadSlides(quizTitle);
        ElementHander.currentOpenQuizTitle = quizTitle;
    }

    private void LoadSlides(string quizTitle)
    {
        if(modifyQuizHandler != null)
        {
            modifyQuizHandler.slides = QuizDataHandler.GetAllQuizSlides(quizTitle);
            QuizOverview.Child = modifyQuizHandler.InitQuizOverview();
        }
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
                    LoadSlides(ElementHander.currentOpenQuizTitle ?? "");
                    modifyQuizHandler?.OpenSlideById(slideId);
                }
            }
        });
        MainGrid.Children.Add(dialogGrid);
    }

    private void DeleteSlideClick(object? sender, RoutedEventArgs e)
    {
        if(modifyQuizHandler?.currentSelectedSlide != null)
        {
            var dialog = Dialog.AreYouSure( MainGrid, $"When you press the confim button slide '{modifyQuizHandler.currentSelectedSlideTypeIndex}' will be deleted.", () =>
            {
                bool deleteResult = QuizDataHandler.DeleteSlide(ElementHander.currentOpenQuizTitle ?? "", modifyQuizHandler.currentSelectedSlide.Id);
                if (deleteResult)
                {
                    bool reassignResult = QuizDataHandler.ReassignSlideIds(ElementHander.currentOpenQuizTitle ?? ""); 
                    if(reassignResult && ElementHander.currentOpenQuizTitle != null) LoadSlides(ElementHander.currentOpenQuizTitle);
                } 
            });
            MainGrid.Children.Add(dialog);
        }
    }


    private void BackClick(object? sender, RoutedEventArgs e)
    {
        modifyQuizHandler?.WriteNewQuestionData();
        
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new QuizLibraryScreen();
            }
        }
    }


}
