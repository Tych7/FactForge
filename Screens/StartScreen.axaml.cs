using System;
using System.Net.Sockets;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Microsoft.AspNetCore.SignalR;

namespace DesktopApp;

public partial class StartScreen : UserControl
{
    public Bitmap QrImage { get; }
    public string JoinUrl { get; }

    public StartScreen()
    {
        var hub = new QuizHub();

        InitializeComponent();

        var ip = hub.GetLocalIPAddress();
        JoinUrl = $"http://{ip}:5000";
        QrImage = hub.GenerateQrCode(JoinUrl);

        QrImageControl.Source = QrImage;
        JoinUrlText.Text = JoinUrl;
    }

    private async void SendQuestionClick(object? sender, RoutedEventArgs e)
    {
        if (App.HubContext != null)
        {
            var q = QuizManager.GetNextQuestion();

            await App.HubContext.Clients.All.SendAsync(
                "NewQuestion",
                new
                {
                    type = q.Type,       
                    text = q.Question,
                    choices = q.Answers,
                    time = q.Time
                }
            );
        }
    }

    private void ToLibraryScreenClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new QuizLibraryScreen();
            }
        }
    }

    private void ToSettingsClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new Settings();
            }
        }
    }


    private void ExitClick(object? sender, RoutedEventArgs args)
    {
        Environment.Exit(0);
    }

    
}
