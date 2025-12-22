using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using static DesktopApp.AppHandler;

namespace DesktopApp;

public partial class Settings : UserControl
{
    private DisplayMode currentWindowMode;
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
        List<string> options = new List<string> { DisplayMode.FullScreen.ToString(), DisplayMode.Windowed.ToString() };
        (Border dropDownElement, ComboBox dropDown) = DropdownElement.Create(options, currentWindowMode.ToString(), 500, 50);

        dropDown.SelectionChanged += (_, __) =>
        {
            if (dropDown.SelectedItem is string selected)
            {
                switch (selected)
                {
                    case "FullScreen":
                        currentWindowMode = DisplayMode.FullScreen;
                        break;
                    case "Windowed":
                        currentWindowMode = DisplayMode.Windowed;
                        break;
                }
            }
        };

        FullscreenSetting.Children.Add(dropDownElement);
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

    private DisplayMode GetCurrentWindowMode()
    {
        var window = VisualRoot as Window;
        if (window == null)
            return DisplayMode.Unknown;

        return window.WindowState == WindowState.FullScreen ? DisplayMode.FullScreen : DisplayMode.Windowed;
    }
    
}
