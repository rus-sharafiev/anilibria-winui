using Anilibria.Core.Models;
using Anilibria.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.WinUI.UI.Animations;

namespace Anilibria.Views;

public sealed partial class ReleasePage : Page
{
    public ReleaseViewModel ViewModel { get; }

    public ReleasePage()
    {
        ViewModel = App.GetService<ReleaseViewModel>();
        InitializeComponent();

        ViewModel.VideoPlayerElement = VideoPlayer;
        ViewModel.PlayerContainer = PlayerContainer;
        ViewModel.VideoContainer = VideoContainer;
        ViewModel.DispatcherQueue = DispatcherQueue;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is Release release && release.IsAnimationAllowed)
        {
            this.RegisterElementForConnectedAnimation("listItemKey", releasePoster);
        }
    }
}
