using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Anilibria.Helpers;

namespace Anilibria;

public sealed partial class PlayerWindow : Window
{
    private static AppWindow? ThisAppWindow;

    public PlayerWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "AppDisplayName".GetLocalized();

        // Fullscreen
        var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
        ThisAppWindow = AppWindow.GetFromWindowId(windowId);
        ThisAppWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
    }

    private MediaPlayerElement? Player
    {
        get; set;
    }

    public void InstallPlayer(MediaPlayerElement mediaPlayerElement)
    {
        Player = mediaPlayerElement;
        ControlGrid.Children.Add(Player);
    }

    public MediaPlayerElement? UninstallPlayer()
    {
        ControlGrid.Children.Remove(Player);
        var mediaPlayerElement = Player;
        Player = null;
        return mediaPlayerElement;
    }
}
