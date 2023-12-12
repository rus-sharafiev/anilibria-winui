using Anilibria.Contracts.Services;
using Anilibria.Core.Models;
using Anilibria.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Anilibria.Controls;

public sealed class CustomMediaTransportControls : MediaTransportControls
{
    public event EventHandler<EventArgs>? OnFullWindowClick;

    private readonly IVideoQualitySelectorService _videoQualitySelectorService = App.GetService<IVideoQualitySelectorService>();

    public CustomMediaTransportControls()
    {
        DefaultStyleKey = typeof(CustomMediaTransportControls);
    }

    protected override void OnApplyTemplate()
    {
        // Find the custom button and create an event handler for its Click event.
        if (GetTemplateChild("FullWindowButton") is Button fullWindowButton)
        {
            fullWindowButton.Click += FullWindowButton_Click;
        }
        if (GetTemplateChild("VideoQualityOptionFHD") is MenuFlyoutItem videoQualityOptionFHD)
        {
            videoQualityOptionFHD.Click += VideoQualityOption_Click;
        }
        if (GetTemplateChild("VideoQualityOptionHD") is MenuFlyoutItem videoQualityOptionHD)
        {
            videoQualityOptionHD.Click += VideoQualityOption_Click;
        }
        if (GetTemplateChild("VideoQualityOptionSD") is MenuFlyoutItem videoQualityOptionSD)
        {
            videoQualityOptionSD.Click += VideoQualityOption_Click;
        }

        switch (_videoQualitySelectorService.Qlt)
        {
            case QltString.SD:
                VisualStateManager.GoToState(this, "SdVideoQualityState", false);
                break;
            case QltString.HD:
                VisualStateManager.GoToState(this, "HdVideoQualityState", false);
                break;
            case QltString.FHD:
                VisualStateManager.GoToState(this, "FhdVideoQualityState", false);
                break;
        }

        base.OnApplyTemplate();
    }

    private void FullWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OnFullWindowClick?.Invoke(this, EventArgs.Empty);

        var state = App.MainWindow.Visible ? "NonFullWindowState" : "FullWindowState";
        VisualStateManager.GoToState(this, state, false);
    }

    private void VideoQualityOption_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem selectedItem)
        {
            var qltOption = selectedItem.Text.ToString();

            switch (qltOption)
            {
                case "FHD":
                    _videoQualitySelectorService.SetVideoQualityAsync(QltString.FHD);
                    VisualStateManager.GoToState(this, "FhdVideoQualityState", false);
                    break;
                case "HD":
                    _videoQualitySelectorService.SetVideoQualityAsync(QltString.HD);
                    VisualStateManager.GoToState(this, "HdVideoQualityState", false);
                    break;
                case "SD":
                    _videoQualitySelectorService.SetVideoQualityAsync(QltString.SD);
                    VisualStateManager.GoToState(this, "SdVideoQualityState", false);
                    break;
            }
        }
    }
}
