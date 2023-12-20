using System.Collections.ObjectModel;
using Anilibria.Contracts.Services;
using Anilibria.Contracts.ViewModels;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;
using Anilibria.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace Anilibria.ViewModels;

public partial class ScheduleViewModel : ObservableRecipient, INavigationAware
{
    private readonly IApiService _apiService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private ObservableCollection<GroupedReleases> _groupedReleases = [];

    [ObservableProperty]
    private string? _connectionError;

    public ScheduleViewModel(IApiService apiService, INavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
    }

    public async void LoadSchedule()
    {
        IsLoading = true;
        ConnectionError = null;
        Day[] days = [];
        try
        {
            days = await _apiService.GetScheduleAsync();
        }
        catch (Exception e)
        {
            ConnectionError = e.InnerException?.Message ?? e.Message;
        }
        finally
        {
            IsLoading = false;
        }

        GroupedReleases.Clear();
        foreach (var day in days)
        {
            var newGroup = new GroupedReleases
            {
                GroupTitle = day.Number switch
                {
                    "1" => "Monday".GetLocalized(),
                    "2" => "Tuesday".GetLocalized(),
                    "3" => "Wednesday".GetLocalized(),
                    "4" => "Thursday".GetLocalized(),
                    "5" => "Friday".GetLocalized(),
                    "6" => "Saturday".GetLocalized(),
                    "7" => "Sunday".GetLocalized(),
                    _ => "",
                },
                Releases = new ObservableCollection<Release>(day.Items)
            };
            GroupedReleases.Add(newGroup);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        LoadSchedule();
    }

    public void OnItemClick(object _, ItemClickEventArgs e)
    {
        if (e.ClickedItem is Release release)
        {
            _navigationService.NavigateTo(typeof(ReleaseViewModel).FullName!, release);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}