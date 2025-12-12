using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using static DesktopApp.AppHandler;

namespace DesktopApp;

public partial class Settings : UserControl
{
    private WindowModes currentWindowMode;
    public Settings()
    {
        InitializeComponent();
        
        AttachedToVisualTree += (_, __) =>
        {
            currentWindowMode = GetCurrentWindowMode();
            InitSettings();
        };
        
    }

    private void InitSettings()
    {
        List<string> options = [WindowModes.FullScreen.ToString(), WindowModes.Windowed.ToString()];
        Border dropdownBorder = DropdownElement.Create(options, currentWindowMode.ToString(), 500, 50, OnSelectedWindowMode);

        FullscreenSetting.Children.Add(dropdownBorder);
    }

    private void OnSelectedWindowMode(string selected)
    {
        switch (selected)
        {
            case "FullScreen":
                currentWindowMode = WindowModes.FullScreen;
                break;
            case "Windowed":
                currentWindowMode = WindowModes.Windowed;
                break;
        }
    }

    private void SaveClick(object? sender, RoutedEventArgs e)
    {
        var window = VisualRoot as Window;
        if(window != null) ChangeDisplayMode(window, currentWindowMode);
    }

    private void BackClick(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // NAviogate back to Start screen
            if (desktop.MainWindow is MainWindow mainWindow) mainWindow.MainContent.Content = new StartScreen();
        }
    }

    private void ToggleFullscreenClick(object? sender, RoutedEventArgs e)
    {
        var window = VisualRoot as Window;
        if (window == null)
            return;

        if (window.WindowState == WindowState.FullScreen) window.WindowState = WindowState.Normal;
        else window.WindowState = WindowState.FullScreen;
    }

    private WindowModes GetCurrentWindowMode()
    {
        var window = VisualRoot as Window;
        if (window == null)
            return WindowModes.Unknown;

        return window.WindowState == WindowState.FullScreen ? WindowModes.FullScreen : WindowModes.Windowed;
    }
    
}
