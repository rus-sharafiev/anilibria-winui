using Anilibria.Contracts.Services;
using Anilibria.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Xaml;
using Anilibria.Services;

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
        ViewModel.MediaPlayerElement = MediaPlayer;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("listItemKey", titlePoster);
    }
}
