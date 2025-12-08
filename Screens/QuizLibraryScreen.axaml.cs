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
        foreach (var quizTitle in QuizManager.GetAllQuizTitles()){
            AddNewQuizInstance(quizTitle);
        }
    }

    private void AddNewQuizInstance(string QuizTitle)
    {
        var button = new Button
        {
            Content = QuizTitle,
            Classes = { "neon-text-button" },
            Margin = new Thickness(10, 10, 10, 0),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
        };

        // Optional: attach a click handler
        button.Click += (s, e) =>
        {
            // Handle quiz button click here
            Console.WriteLine($"{QuizTitle} clicked!");
        };

        QuizListPanel.Children.Add(button);
    }

    public void AddQuizButtonClick(object? sender, RoutedEventArgs e)
    {
        AddNewQuizInstance("test");
    }

    public void CreateNewQuizClick(object? sender, RoutedEventArgs e)
    {
        var (newQuizDialog, titleBox) = Dialog.CreateNewQuiz(MainGrid, title =>
        {
            Console.WriteLine("User entered title: " + title);

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
