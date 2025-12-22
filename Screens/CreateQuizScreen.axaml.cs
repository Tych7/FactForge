using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

namespace DesktopApp;

public partial class CreateQuizScreen : UserControl
{
    private string? currentOpenQuizTitle;
    private ModifyQuizHandler? modifyQuizHandler;

    public CreateQuizScreen(string quizTitle)
    {
        InitializeComponent();

        modifyQuizHandler = new ModifyQuizHandler();
        modifyQuizHandler.SetQuizPageGrid(Quizpage);

        LoadSlides(quizTitle);
        currentOpenQuizTitle = quizTitle;
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
        QuizDataHandler.CreateNewQuestion(currentOpenQuizTitle ?? "");
        LoadSlides(currentOpenQuizTitle ?? "");
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
