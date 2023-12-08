using System.Collections.ObjectModel;
using Anilibria.Contracts.ViewModels;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;
using Windows.Media.Core;
using Microsoft.UI.Xaml;
using Windows.Media.Playback;

namespace Anilibria.ViewModels;

public partial class TitleViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Title? title;

    [ObservableProperty]
    private Grid? playerContainer;

    [ObservableProperty]
    private Grid? videoContainer;

    [ObservableProperty]
    private MediaPlaybackList _mediaPlaybackList = new();

    [ObservableProperty]
    private ObservableCollection<string> _episodesList = [];

    [ObservableProperty]
    private QltString _qlt = QltString.HD;

    [ObservableProperty]
    private int _selectedEpisode = 0;

    private readonly IApiService _apiService;

    public TitleViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    #region Navigation
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is Title title)
        {
            Title = title;
            CreateMediaPlaybackList(title);
        }
    }

    public void OnNavigatedFrom()
    {
        PlayerContainer = null;
        VideoContainer = null;
        MediaPlaybackList.Items.Clear();
    }
    #endregion

    private void CreateMediaPlaybackList(Title title)
    {
        MediaPlaybackList.Items.Clear();
        foreach (var entry in title.Player.List)
        {
            var episode = entry.Value.Name is not null ? $". {entry.Value.Name}" : " серия";
            EpisodesList.Add($"{entry.Value.Episode}{episode}");

            var qlt = Qlt switch
            {
                QltString.SD => entry.Value.Hls.Sd,
                QltString.HD => entry.Value.Hls.Hd,
                QltString.FHD => entry.Value.Hls.Fhd,
                _ => null
            };

            if (qlt is not null)
            {
                var uri = new Uri(new Uri($"https://{title.Player.Host}"), qlt);
                var mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(uri));
                MediaPlaybackList.Items.Add(mediaPlaybackItem);
            }
        }
    }

    public void EpisodesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        var comboBox = sender as ComboBox;
        if (comboBox is not null && comboBox.SelectedIndex >= 0)
        {
            System.Diagnostics.Debug.WriteLine(comboBox.SelectedIndex);
            MediaPlaybackList.MoveTo((uint)comboBox.SelectedIndex);
        }
    }

    #region Fullscreen playback
    private PlayerWindow? PlayerWindow { get; set; }

    public MediaPlayerElement? UninstallPlayer()
    {
        if (PlayerContainer is not null)
        {
            var mediaPlayerElement = PlayerContainer.Children.First() as MediaPlayerElement;
            PlayerContainer.Children.Clear();
            return mediaPlayerElement;
        }
        else 
        { 
            return null; 
        }
    }

    public void FullScreenButton_Click(object sender, EventArgs e)
    {
        if (PlayerWindow is not null)
        {
            PlayerWindow.Close();
            PlayerWindow = null;
        }
        else
        {
            if (PlayerContainer is not null && VideoContainer is not null)
            {
                VideoContainer.MinHeight = PlayerContainer.MinHeight;
            }
            App.MainWindow.Hide();
            PlayerWindow = new();

            if (UninstallPlayer() is MediaPlayerElement content)
            {
                PlayerWindow.InstallPlayer(content);
            }

            PlayerWindow.Closed += PlayerWindow_Closed;
            PlayerWindow.Activate();
        }
    }

    private void PlayerWindow_Closed(object sender, WindowEventArgs args)
    {
        if (sender is PlayerWindow playerWindow)
        {
            if (playerWindow.UninstallPlayer() is MediaPlayerElement mediaPlayerElement && PlayerContainer is not null)
            {
                App.MainWindow.Show();
                PlayerContainer.Children.Add(mediaPlayerElement);
            }
        }
    }

    public void PlayerContainer_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (PlayerContainer is not null)
        {
            PlayerContainer.MinHeight = PlayerContainer.MaxHeight = e.NewSize.Width * 9 / 16;
            PlayerContainer.UpdateLayout();
        }
    }
    #endregion
}
