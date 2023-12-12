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

        ViewModel.VideoPlayerElement = VideoPlayer;
        ViewModel.PlayerContainer = PlayerContainer;
        ViewModel.VideoContainer = VideoContainer;
        ViewModel.DispatcherQueue = DispatcherQueue;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("listItemKey", titlePoster);
    }
}
