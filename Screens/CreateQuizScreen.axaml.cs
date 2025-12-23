using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

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

    private void NewPageClick(object? sender, RoutedEventArgs e)
    {
        QuizDataHandler.CreateNewSlide(ElementHander.currentOpenQuizTitle ?? "");
        LoadSlides(ElementHander.currentOpenQuizTitle ?? "");
    }

    private void DeletePageClick(object? sender, RoutedEventArgs e)
    {
        if(modifyQuizHandler?.currentSelectedSlide != null)
        {
            var dialog = Dialog.AreYouSure( MainGrid, $"When you press the confim button slide '{modifyQuizHandler.currentSelectedSlideTypeIndex}' will be deleted.", () =>
            {
                bool result = QuizDataHandler.DeleteSlide(ElementHander.currentOpenQuizTitle ?? "", modifyQuizHandler.currentSelectedSlide.Id);
                Console.WriteLine(modifyQuizHandler.currentSelectedSlide.Id);
                if (result && ElementHander.currentOpenQuizTitle != null) LoadSlides(ElementHander.currentOpenQuizTitle);
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
