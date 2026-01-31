using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

namespace DesktopApp;

public partial class QuizLibraryScreen : UserControl
{
    private StackPanel? libStackPanel;
    private enum quizFilter
    {
        alphabetical,
        favorite,
        accessed
    }
    public QuizLibraryScreen()
    {
        InitializeComponent();
        InitQuizLibScrollView();
        InitQuizLibDetails();
    }

    private void InitQuizLibScrollView()
    {
        QuizLibScrollView.Children.Clear();
        ScrollViewer libScrollView = new ScrollViewer
        {
            VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled
        };
        
        libStackPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Vertical,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
        };

        var allQuizTitles = QuizDataHandler.GetAllQuizTitles();
        for (int i = 0; i < allQuizTitles.Count; i++)
        {
            if(i == 0) QuizLibDetailsElement.selectedQuizTitle = allQuizTitles[i];
            var quizLibScrollElement = QuizLibScrollElement.Create(i, allQuizTitles[i], onQuizClick);
            libStackPanel.Children.Add(quizLibScrollElement);
        }
        libScrollView.Content = libStackPanel;

        Grid quizLibScrollViewElement = QuizLibScrollViewElement.Create(600, libScrollView, OnTabOneClick, OnTabTwoClick, OnTabThreeClick);
        QuizLibScrollView.Children.Add(quizLibScrollViewElement);

        markSelectedQuiz(0);
    }

    private void InitQuizLibDetails()
    {
        QuizInteraction.Children.Clear();
        Border quizLibDetailsElement = QuizLibDetailsElement.Create(450, DeleteQuizClick, EditQuizClick, StartClick);
        QuizInteraction.Children.Add(quizLibDetailsElement);
    }

    private void StartClick()
    {
        Console.WriteLine($"Starting quiz '{QuizLibDetailsElement.selectedQuizTitle}'");
    }

    private void onQuizClick(int id, string quizTitle)
    {
        QuizLibDetailsElement.selectedQuizTitle = quizTitle;
        markSelectedQuiz(id);

        InitQuizLibDetails();
    }

    private void markSelectedQuiz(int id)
    {
        if(libStackPanel != null)
        {
            foreach(var child in libStackPanel.Children)
            {
                if (child is Border border && border.Child != null)
                {
                    if(border.Child is Button button && button.Name != null)
                    {
                        if (int.Parse(button.Name) == id) button.BorderThickness = new Thickness(3);
                        else button.BorderThickness = new Thickness(0);
                    }
                }
            }
        }
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

    private void DeleteQuizClick()
    {
        var dialog = Dialog.AreYouSure( MainGrid, $"When you press the confim button the quiz '{QuizLibDetailsElement.selectedQuizTitle}' will be deleted.", () =>
        {
            if(QuizLibDetailsElement.selectedQuizTitle != null)
            {
                bool result = QuizDataHandler.DeleteQuiz(QuizLibDetailsElement.selectedQuizTitle);
                if (result)
                {
                    InitQuizLibScrollView();
                    InitQuizLibDetails();
                } 
            } 
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

    private void EditQuizClick()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //Navigate to Create Quiz screen
                if (desktop.MainWindow is MainWindow mainWindow && QuizLibDetailsElement.selectedQuizTitle != null) mainWindow.MainContent.Content = new CreateQuizScreen(QuizLibDetailsElement.selectedQuizTitle);
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
