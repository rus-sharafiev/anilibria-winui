using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Anilibria.Controls;

public sealed class CustomMediaTransportControls : MediaTransportControls
{
    public event EventHandler<EventArgs>? OnFullWindowClick;

    public CustomMediaTransportControls()
    {
        DefaultStyleKey = typeof(CustomMediaTransportControls);
    }

    protected override void OnApplyTemplate()
    {
        // Find the custom button and create an event handler for its Click event.
        var fullWindowButton = GetTemplateChild("FullWindowButton") as Button;
        if (fullWindowButton is not null)
        {
            fullWindowButton.Click += FullWindowButton_Click;
        }
        base.OnApplyTemplate();
    }

    private void FullWindowButton_Click(object sender, RoutedEventArgs e)
    {
        OnFullWindowClick?.Invoke(this, EventArgs.Empty);

        var state = App.MainWindow.Visible ? "NonFullWindowState" : "FullWindowState";
        VisualStateManager.GoToState(this, state, false);
    }

}
