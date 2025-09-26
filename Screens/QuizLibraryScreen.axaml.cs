using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

namespace DesktopApp;

public partial class QuizLibraryScreen : UserControl
{
    public QuizLibraryScreen()
    {
        InitializeComponent();
    }

    public void AddQuizButtonClick(object? sender, RoutedEventArgs e)
    {
        var button = new Button
        {
            Content = "test",
            Width = 200,
            Classes = { "neon-square" } // applies your neon style
        };

        // Optional: attach a click handler
        button.Click += (s, e) =>
        {
            // Handle quiz button click here
            Console.WriteLine($"{"test"} clicked!");
        };

        QuizListPanel.Children.Add(button);
    }

    public void CreateNewQuizClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new CreateQuizScreen();
            }
        }
    }


    private void BackClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new StartScreen();
            }
        }
    }


}
