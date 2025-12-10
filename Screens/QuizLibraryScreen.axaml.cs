using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace DesktopApp;

public partial class QuizLibraryScreen : UserControl
{
    public QuizLibraryScreen()
    {
        InitializeComponent();
        InitQuizScrollView();
    }

    private void InitQuizScrollView()
    {
        QuizListPanel.Children.Clear();
        foreach (var quizTitle in QuizDataHandler.GetAllQuizTitles()){
            AddNewQuizInstance(quizTitle);
        }
    }

    private void AddNewQuizInstance(string QuizTitle)
    {
        var quizLibScrollElement = QuizLibScrollElement.Create(QuizTitle, deleteQuizClick, editQuizClick);
        QuizListPanel.Children.Add(quizLibScrollElement);
    }

    private void editQuizClick(string QuizTitle)
    {
        Console.WriteLine("edit button clicked!");
    }

    private void deleteQuizClick(string QuizTitle)
    {
        var dialog = Dialog.AreYouSure( MainGrid, $"When you press the confim button the quiz '{QuizTitle}' will be deleted.", () =>
        {
            bool result = QuizDataHandler.DeleteQuiz(QuizTitle);
            if (result) InitQuizScrollView();
        });
        MainGrid.Children.Add(dialog);
    }


    public void AddQuizButtonClick(object? sender, RoutedEventArgs e)
    {
        AddNewQuizInstance("test");
    }

    public void CreateNewQuizClick(object? sender, RoutedEventArgs e)
    {
        var (newQuizDialog, titleBox) = Dialog.CreateNewQuiz(MainGrid, "Add Quiz", "Title", title =>
        {
            QuizDataHandler.CreateQuiz(title);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //Navigate to Create Quiz screen
                if (desktop.MainWindow is MainWindow mainWindow) mainWindow.MainContent.Content = new CreateQuizScreen();
            }
        });

        MainGrid.Children.Add(newQuizDialog);
    }

    private void BackClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // NAviogate back to Start screen
            if (desktop.MainWindow is MainWindow mainWindow) mainWindow.MainContent.Content = new StartScreen();
        }
    }


}
