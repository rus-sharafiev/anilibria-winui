using System.Collections.ObjectModel;
using Anilibria.Contracts.Services;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;
using Anilibria.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

namespace Anilibria.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    [ObservableProperty]
    private ObservableCollection<Release> _autoSuggestionBoxItemsSource = [];

    [ObservableProperty]
    private bool _isReleaseNavigationViewItemEnabled = false;

    [ObservableProperty]
    private PersonPicture _userPicture = new();

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    private readonly IApiService _apiService;
    private readonly IUserService _userService;
    public ShellViewModel(
        IApiService apiService, 
        IUserService userService,
        INavigationService navigationService, 
        INavigationViewService navigationViewService
        )
    {
        _apiService = apiService;
        _userService = userService;
        _userService.UserChanged += UserChanged;
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }

        IsReleaseNavigationViewItemEnabled = e.SourcePageType == typeof(ReleasePage);
    }

    public void IsReleaseNavigationViewItemEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is NavigationViewItem item)
        {
            item.Opacity = item.IsEnabled ? 1.0 : 0;
        }
    }

    #region AutoSuggestionBox events
    private Release[] _searchData = [];
    public async void SuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text.Length > 3)
            {
                var data = await _apiService.SearchAsync(sender.Text);
                if (!_searchData.SequenceEqual(data))
                {
                    _searchData = data;
                    AutoSuggestionBoxItemsSource.Clear();
                    foreach (var release in _searchData)
                    {
                        AutoSuggestionBoxItemsSource.Add(release);
                    }
                }
            }
            else AutoSuggestionBoxItemsSource.Clear();
        }
        else AutoSuggestionBoxItemsSource.Clear();
    }

    public void SuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is Release release)
        {
            //sender.Text = title.Names.Ru;
            System.Diagnostics.Debug.WriteLine("selected title: " + release.Names.First());
        }
        else
        {
            // Use args.QueryText to determine what to do.
            System.Diagnostics.Debug.WriteLine("plain text: " + sender.Text);
        }
    }

    public void SuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is Release release)
        {
            // User selected an item from the suggestion list, take an action on it here.
            System.Diagnostics.Debug.WriteLine("selected title: " + release.Names.First());
            release.IsAnimationAllowed = false;
            NavigationService.NavigateTo(typeof(ReleaseViewModel).FullName!, release);
        }
    }
    #endregion

    #region Person picture
    public void PersonPicture_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is PersonPicture personPicture)
        {
            UserPicture = personPicture;
            SetUserPicture();
        }
    }

    private void UserChanged(object? sender, EventArgs e) => SetUserPicture();
    private void SetUserPicture()
    {
        if (_userService.User is not null)
        {
            var imagePath = _userService.User.Avatar ?? "/upload/avatars/noavatar.jpg";
            BitmapImage bitmapImage = new();
            bitmapImage.UriSource = new Uri(new Uri("https://static.wwnd.space"), imagePath);
            UserPicture.ProfilePicture = bitmapImage;
        }
        else
        {
            UserPicture.ProfilePicture = null;
        }
    }
    #endregion
}
