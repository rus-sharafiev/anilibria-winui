﻿using System.Collections.ObjectModel;
using Anilibria.Contracts.ViewModels;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;
using Windows.Media.Core;
using Microsoft.UI.Xaml;
using Windows.Media.Playback;
using Microsoft.UI.Dispatching;
using Anilibria.Contracts.Services;

namespace Anilibria.ViewModels;

public partial class TitleViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Title? _title;

    [ObservableProperty]
    private MediaPlayerElement _videoPlayerElement = new();

    [ObservableProperty]
    private Grid? _playerContainer;

    [ObservableProperty]
    private Grid? _videoContainer;

    [ObservableProperty]
    private ObservableCollection<string> _episodesList = [];

    [ObservableProperty]
    private int _selectedEpisode = 0;

    [ObservableProperty]
    private DispatcherQueue? _dispatcherQueue;

    private readonly MediaPlayer _mediaPlayer = new();
    private MediaPlaybackList _mediaPlaybackList = new();

    private readonly IApiService _apiService;
    private readonly IVideoQualitySelectorService _videoQualitySelectorService;

    public TitleViewModel(IApiService apiService, IVideoQualitySelectorService videoQualitySelectorService)
    {
        _apiService = apiService;
        _videoQualitySelectorService = videoQualitySelectorService;
        _videoQualitySelectorService.QualityChanged += VideoQuality_QualityChanged;
    }

    #region Navigation
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is Title title)
        {
            Title = title;

            // Create episodes list
            foreach (var entry in title.Player.List)
            {
                var episode = entry.Value.Name is not null ? $". {entry.Value.Name}" : " серия";
                EpisodesList.Add($"{entry.Value.Episode}{episode}");
            }
            CreateMediaPlaybackList();
        }
    }

    public void OnNavigatedFrom()
    {
        VideoPlayerElement.SetMediaPlayer(null);
        _mediaPlayer.Dispose();
        VideoContainer = null;
    }
    #endregion

    private void VideoQuality_QualityChanged(object? sender, EventArgs e) => CreateMediaPlaybackList(true);

    private void CreateMediaPlaybackList(bool videoQualityChanged = false)
    {
        if (Title is not null && VideoPlayerElement is not null)
        {

            // Get curent player state
            var selectedIndex = SelectedEpisode;
            var isPlaying = false;
            var playbackSessionPosition = TimeSpan.Zero;
            if (videoQualityChanged)
            {
                isPlaying = _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing;
                playbackSessionPosition = _mediaPlayer.PlaybackSession.Position;
            }

            // Create new MediaPlaybackList
            _mediaPlaybackList = new MediaPlaybackList { MaxPrefetchTime = TimeSpan.Zero };
            foreach (var entry in Title.Player.List)
            {
                var hls = _videoQualitySelectorService.Qlt switch
                {
                    QltString.SD => entry.Value.Hls.Sd,
                    QltString.HD => entry.Value.Hls.Hd,
                    QltString.FHD => entry.Value.Hls.Fhd,
                    _ => null
                };

                if (hls is not null)
                {
                    var uri = new Uri(new Uri($"https://{Title.Player.Host}"), hls);
                    var mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(uri));
                    _mediaPlaybackList.Items.Add(mediaPlaybackItem);
                }
            }
            _mediaPlayer.Source = _mediaPlaybackList;
            _mediaPlaybackList.CurrentItemChanged += MediaPlaybackList_CurrentItemChanged;
            _mediaPlaybackList.ItemOpened += MediaPlaybackList_ItemOpened;
            VideoPlayerElement.SetMediaPlayer(_mediaPlayer);

            // Restore player state on video quality changed
            if (videoQualityChanged)
            {
                _mediaPlaybackList.MoveTo((uint)selectedIndex);
                _mediaPlayer.PlaybackSession.Position = playbackSessionPosition;
                if (isPlaying)
                {
                    _mediaPlayer.Play();
                }
            }
        }
    }

    private void MediaPlaybackList_ItemOpened(MediaPlaybackList sender, MediaPlaybackItemOpenedEventArgs args) =>
        System.Diagnostics.Debug.WriteLine($"Item has been opened");

    private void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
    {
        var newItemIdex = sender.Items.IndexOf(args.NewItem);
        System.Diagnostics.Debug.WriteLine($"Selected: {SelectedEpisode}, new: {newItemIdex}");
        if (SelectedEpisode != newItemIdex && newItemIdex >= 0)
        {
            System.Diagnostics.Debug.WriteLine("CurrentItemChanged");
            DispatcherQueue?.TryEnqueue(DispatcherQueuePriority.Normal, () =>
            {
                SelectedEpisode = newItemIdex;
            });
        }
    }

    public void EpisodesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedIndex != SelectedEpisode && comboBox.SelectedIndex >= 0)
        {
            System.Diagnostics.Debug.WriteLine("SelectionChanged");
            _mediaPlaybackList.MoveTo((uint)comboBox.SelectedIndex);
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