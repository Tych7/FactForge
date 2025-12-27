

using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace DesktopApp;

public class AppHandler
{
    public enum DisplayMode
    {
        Unknown,
        FullScreen,
        Windowed
    }

    public AppHandler()
    {

    }

    public static void ChangeDisplayMode(Window window, DisplayMode mode)
    {
        switch (mode)
        {
            case DisplayMode.FullScreen:
                window.WindowState = WindowState.FullScreen;
                break;
            case DisplayMode.Windowed:
                window.WindowState = WindowState.Normal;
                break;
        }
    }

    public static Geometry GetIcon(string key)
    {
        if (Application.Current?.TryGetResource(
                key,
                Application.Current.ActualThemeVariant,
                out var res) == true
            && res is Geometry g)
            return g;

        throw new KeyNotFoundException($"Icon '{key}' not found.");
    }

}