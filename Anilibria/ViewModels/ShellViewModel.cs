using System.Collections.ObjectModel;
using Anilibria.Contracts.Services;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;
using Anilibria.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Anilibria.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    [ObservableProperty]
    private ObservableCollection<Title> _autoSuggestionBoxItemsSource = [];

    [ObservableProperty]
    private bool _isTitleNavigationViewItemEnabled = false;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    private readonly IApiService _apiService;
    public ShellViewModel(IApiService apiService, INavigationService navigationService, INavigationViewService navigationViewService)
    {
        _apiService = apiService;
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

        IsTitleNavigationViewItemEnabled = e.SourcePageType == typeof(TitlePage);
    }

    public void IsTitleNavigationViewItemEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is NavigationViewItem item)
        {
            item.Opacity = item.IsEnabled ? 1.0 : 0;
        }
    }

    #region AutoSuggestionBox events
    private Title[] _searchData = [];
    public async void SuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text.Length > 3)
            {
                var data = await _apiService.SearchTitles(sender.Text);
                if (!_searchData.SequenceEqual(data.List))
                {
                    _searchData = data.List;
                    AutoSuggestionBoxItemsSource.Clear();
                    foreach (var title in _searchData)
                    {
                        AutoSuggestionBoxItemsSource.Add(title);
                    }
                }
            }
            else AutoSuggestionBoxItemsSource.Clear();
        }
        else AutoSuggestionBoxItemsSource.Clear();
    }

    public void SuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is Title title)
        {
            //sender.Text = title.Names.Ru;
            System.Diagnostics.Debug.WriteLine("selected title: " + title.Names.Ru);
        }
        else
        {
            // Use args.QueryText to determine what to do.
            System.Diagnostics.Debug.WriteLine("plain text: " + sender.Text);
        }
    }

    public void SuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is Title title)
        {
            // User selected an item from the suggestion list, take an action on it here.
            System.Diagnostics.Debug.WriteLine("selected title: " + title.Names.Ru);
            title.IsAnimationAllowed = false;
            NavigationService.NavigateTo(typeof(TitleViewModel).FullName!, title);
        }
    }
    #endregion
}
