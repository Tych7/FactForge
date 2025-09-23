using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DesktopApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartClick(object? sender, RoutedEventArgs args)
    {
        MainContent.Content = new StartScreen();
    }
}
