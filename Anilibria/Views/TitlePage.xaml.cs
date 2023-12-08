using Anilibria.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using CommunityToolkit.WinUI.UI.Animations;
using Windows.Media.Playback;

namespace Anilibria.Views;

public sealed partial class TitlePage : Page
{
    public TitleViewModel ViewModel { get; }

    public TitlePage()
    {
        ViewModel = App.GetService<TitleViewModel>();
        InitializeComponent();

        ViewModel.PlayerContainer = PlayerContainer;
        ViewModel.VideoContainer = VideoContainer;
        ViewModel.MediaPlaybackList.CurrentItemChanged += MediaPlaybackList_CurrentItemChanged;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("listItemKey", titlePoster);
    }

    private void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args) =>
        DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
        {
            if (ViewModel.SelectedEpisode != (int)sender.CurrentItemIndex && (int)sender.CurrentItemIndex >= 0)
            {
                ViewModel.SelectedEpisode = (int)sender.CurrentItemIndex;
            }
        });
}
