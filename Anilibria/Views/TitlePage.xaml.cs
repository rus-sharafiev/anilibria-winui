using Anilibria.Core.Models;
using Anilibria.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.WinUI.UI.Animations;

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
        if (e.Parameter is Title title && title.IsAnimationAllowed)
        {
            this.RegisterElementForConnectedAnimation("listItemKey", titlePoster);
        }
    }
}
