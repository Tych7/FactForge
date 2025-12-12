

using Avalonia.Controls;
using Avalonia.Interactivity;

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
}