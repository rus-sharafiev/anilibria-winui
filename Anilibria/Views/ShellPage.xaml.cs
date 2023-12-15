using Anilibria.Contracts.Services;
using Anilibria.Helpers;
using Anilibria.ViewModels;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.System;
using Windows.UI.WindowManagement;

namespace Anilibria.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel { get; }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        AppTitleBar.Loaded += AppTitleBar_Loaded;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = AppTitleBarText;

        if (args.WindowActivationState == WindowActivationState.Deactivated)
        {
            AppTitleBarText.Foreground =
                (SolidColorBrush)App.Current.Resources["WindowCaptionForegroundDisabled"];
        }
        else
        {
            AppTitleBarText.Foreground =
                (SolidColorBrush)App.Current.Resources["WindowCaptionForeground"];
        }
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        var scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = App.MainWindow.AppWindow.TitleBar.RightInset / scaleAdjustment,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    #region Specify the interactive regions of the title bar
    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e) => SetRegionsForCustomTitleBar();

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e) => SetRegionsForCustomTitleBar();

    private void SetRegionsForCustomTitleBar()
    {
        var scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

        // Get the rectangle around the AutoSuggestBox control.
        var transform = AppTitleBarSearchBox.TransformToVisual(null);
        var bounds = transform.TransformBounds(new Rect(0, 0,
                                                        AppTitleBarSearchBox.ActualWidth,
                                                        AppTitleBarSearchBox.ActualHeight));
        var SearchBoxRect = GetRect(bounds, scaleAdjustment);

        // Get the rectangle around the PersonPicture control.
        transform = PersonPic.TransformToVisual(null);
        bounds = transform.TransformBounds(new Rect(0, 0,
                                                    PersonPic.ActualWidth,
                                                    PersonPic.ActualHeight));
        var PersonPicRect = GetRect(bounds, scaleAdjustment);

        var rectArray = new Windows.Graphics.RectInt32[] { SearchBoxRect, PersonPicRect };

        var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(App.MainWindow.AppWindow.Id);
        nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
    }

    private static Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
    {
        return new Windows.Graphics.RectInt32(
            _X: (int)Math.Round(bounds.X * scale),
            _Y: (int)Math.Round(bounds.Y * scale),
            _Width: (int)Math.Round(bounds.Width * scale),
            _Height: (int)Math.Round(bounds.Height * scale)
        );
    }
    #endregion

    #region KeyboardAccelerator
    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }
    #endregion

    #region AppTitleBarSearchBox events
    private void TitleBarSearchBoxButton_Click(object sender, RoutedEventArgs e)
    {
        TitleBarSearchBoxButton.Visibility = Visibility.Collapsed;
        TitleBarAutoSuggestBox.Opacity = 1.0;
        TitleBarAutoSuggestBox.Focus(FocusState.Pointer);
    }

    private void TitleBarAutoSuggestBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
    {
        TitleBarAutoSuggestBox.Opacity = 0;
        TitleBarAutoSuggestBox.Text = string.Empty;
        TitleBarSearchBoxButton.Visibility = Visibility.Visible;
    }
    #endregion
}
