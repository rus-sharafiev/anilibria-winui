﻿using System.Collections.ObjectModel;
using Anilibria.Contracts.Services;
using Anilibria.Contracts.ViewModels;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;
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
    private ObservableCollection<GroupedTitles> titlesGroups = [];

    [ObservableProperty]
    private string? _connectionError;

    public ScheduleViewModel(IApiService apiService, INavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        IsLoading = true; 
        ConnectionError = null;
        List<TitlesByDay> data = [];
        try
        {
            data = await _apiService.GetScheduleAsync();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Processing failed: {e.Message}");
            ConnectionError = e.Message;
        }
        finally
        {
            IsLoading = false;
        }

        TitlesGroups.Clear();
        foreach (var item in data)
        {
            var newGroup = new GroupedTitles
            {
                GroupTitle = item.Day switch
                {
                    0 => "Понедельник",
                    1 => "Вторник",
                    2 => "Среда",
                    3 => "Четверг",
                    4 => "Пятница",
                    5 => "Суббота",
                    6 => "Воскресенье",
                    _ => "",
                },
                Titles = new ObservableCollection<Title>(item.List)
            };
            TitlesGroups.Add(newGroup);
        }
    }

    public void OnItemClick(object _, ItemClickEventArgs e)
    {
        var title = e.ClickedItem as Title;
        if (title is not null)
        {
            _navigationService.NavigateTo(typeof(TitleViewModel).FullName!, title);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}