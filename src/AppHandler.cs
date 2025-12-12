

using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DesktopApp;

public class AppHandler
{
    public enum WindowModes
    {
        Unknown,
        FullScreen,
        Windowed
    }

    public AppHandler()
    {

    }

    public static void ChangeDisplayMode(Window window, WindowModes mode)
    {
        switch (mode)
        {
            case WindowModes.FullScreen:
                window.WindowState = WindowState.FullScreen;
                break;
            case WindowModes.Windowed:
                window.WindowState = WindowState.Normal;
                break;
        }
    }
}