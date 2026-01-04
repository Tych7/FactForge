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
        ScrollViewer libScrollView = new ScrollViewer
        {
            VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled
        };
        
        StackPanel libStackPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Vertical,
            Width = 550
        };
        foreach (var quizTitle in QuizDataHandler.GetAllQuizTitles()){
            var quizLibScrollElement = QuizLibScrollElement.Create(quizTitle, DeleteQuizClick, EditQuizClick);
            libStackPanel.Children.Add(quizLibScrollElement);
        }
        libScrollView.Content = libStackPanel;
        Grid quizLibTabsElement = QuizLibTabsElement.Create(500, 650, libScrollView, OnTabOneClick, OnTabTwoClick, OnTabThreeClick);

        QuizListPanel.Children.Add(quizLibTabsElement);
    }

    private void OnTabOneClick()
    {
        
    }

    private void OnTabTwoClick()
    {
        
    }

    private void OnTabThreeClick()
    {
        
    }

    private void DeleteQuizClick(string QuizTitle)
    {
        var dialog = Dialog.AreYouSure( MainGrid, $"When you press the confim button the quiz '{QuizTitle}' will be deleted.", () =>
        {
            bool result = QuizDataHandler.DeleteQuiz(QuizTitle);
            if (result) InitQuizScrollView();
        });
        MainGrid.Children.Add(dialog);
    }

    public void CreateNewQuizClick(object? sender, RoutedEventArgs e)
    {
        var (newQuizDialog, titleBox) = Dialog.CreateNewQuiz(MainGrid, "Add Quiz", "Title", title =>
        {
            QuizDataHandler.CreateQuiz(title);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //Navigate to Create Quiz screen
                if (desktop.MainWindow is MainWindow mainWindow) mainWindow.MainContent.Content = new CreateQuizScreen(title);
            }
        });

        MainGrid.Children.Add(newQuizDialog);
    }

    private void EditQuizClick(string QuizTitle)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //Navigate to Create Quiz screen
                if (desktop.MainWindow is MainWindow mainWindow) mainWindow.MainContent.Content = new CreateQuizScreen(QuizTitle);
            }
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
