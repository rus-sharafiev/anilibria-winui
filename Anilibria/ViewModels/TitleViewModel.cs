using System.Collections.ObjectModel;
using Anilibria.Contracts.ViewModels;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;
using Windows.Media.Core;
using Microsoft.UI.Xaml;


namespace Anilibria.ViewModels;

public partial class TitleViewModel : ObservableRecipient, INavigationAware
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private Title? title;

    [ObservableProperty]
    private string? releaseDateTime;

    [ObservableProperty]
    private MediaSource? videoSource;

    [ObservableProperty]
    private int selectedEpisode = 0;

    [ObservableProperty]
    private int selectedQlt = 1;

    [ObservableProperty]
    private Grid? playerContainer;

    [ObservableProperty]
    private Grid? videoContainer;

    [ObservableProperty]
    private MediaPlayerElement? mediaPlayerElement;

    public ObservableCollection<Tuple<string, ListValue>> EpisodesList { get; } = [];
    public ObservableCollection<Tuple<string, string>> QualityList { get; } = [];

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
            ReleaseDateTime = DateTimeOffset.FromUnixTimeSeconds(title.LastChange).ToString("G");
            foreach (var entry in title.Player.List)
            {
                var episode = entry.Value.Name is not null ? $". {entry.Value.Name}" : " серия";
                EpisodesList.Add(new Tuple<string, ListValue>($"{entry.Value.Episode}{episode}", entry.Value));
            }
        }
    }

    public void OnNavigatedFrom()
    {
        PlayerContainer = null;
        VideoContainer = null;
        if (MediaPlayerElement is not null)
        {
            MediaPlayerElement.Source = null;
            MediaPlayerElement = null;
        }
    }
    #endregion

    public void EpisodesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        QualityList.Clear();
        foreach (var video in args.AddedItems.Cast<Tuple<string, ListValue>>())
        {
            foreach (var property in video.Item2.Hls.GetType().GetProperties())
            {
                var value = property.GetValue(video.Item2.Hls) as string;
                if (value is not null)
                {
                    QualityList.Add(new Tuple<string, string>(property.Name.ToUpper(), value));
                }
                SelectedQlt = 1;
            }
        }
    }

    public void VideoQualityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        foreach (var source in args.AddedItems.Cast<Tuple<string, string>>())
        {
            if (Title is not null)
            {
                var uri = new Uri(new Uri($"https://{Title.Player.Host}"), source.Item2);
                VideoSource = MediaSource.CreateFromUri(uri);
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
    #endregion
}
