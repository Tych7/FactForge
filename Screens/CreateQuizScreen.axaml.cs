using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

namespace DesktopApp;

public partial class CreateQuizScreen : UserControl
{
    public CreateQuizScreen()
    {
        InitializeComponent();
    }

    private void NewPageClick(object? sender, RoutedEventArgs e)
    {
        var openQuestion = new OpenQuestionElement();
        Quizpage.Children.Add(openQuestion.Create());
    }

    private void BackClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new QuizLibraryScreen();
            }
        }
    }


}
