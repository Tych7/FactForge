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
    public CreateQuizScreen()
    {
        InitializeComponent();
        modifyQuizHandler = new ModifyQuizHandler();
        modifyQuizHandler.SetQuizPageGrid(Quizpage);
    }

    public CreateQuizScreen(string quizTitle)
    {
        InitializeComponent();

        modifyQuizHandler = new ModifyQuizHandler();
        modifyQuizHandler.SetQuizPageGrid(Quizpage);

        LoadSlides(quizTitle);

    }

    private void LoadSlides(string quizTitle)
    {
        if(modifyQuizHandler != null)
        {
            modifyQuizHandler.slides = QuizDataHandler.GetAllQuizSlides(quizTitle);
            QuizOverview.Child = modifyQuizHandler.CreateQuizOverview();
        }
    }

    private void NewPageClick(object? sender, RoutedEventArgs e)
    {
        
    }

    private void OpenPanelClick(object? sender, RoutedEventArgs e)
    {
        StackPanel test = new StackPanel{};

        for(int i = 0; i < 20; i++)
        {
            TextBlock testText = new TextBlock
            {
                Text = "test",
                Classes = { "neon-text" },
                Margin = new Thickness(0, 20, 0, 0),
                FontSize = 50
            };
            test.Children.Add(testText);
        }


        var optionPanel = SlidingPanelElement.Create(MainGrid, test, "OPTIONS", 500);
        MainGrid.Children.Add(optionPanel);
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
