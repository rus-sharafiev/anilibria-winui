using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;

namespace Anilibria.Core.Services;

public class ApiService : IApiService
{
    private readonly HttpDataService _instance = new("https://api.anilibria.srrlab.ru");

    public ApiService()
    {
    }

    private List<TitlesByDay> _scheduleData;
    public async Task<List<TitlesByDay>> GetScheduleAsync(bool forceRefresh = false)
    {
        _scheduleData = await _instance.GetAsync<List<TitlesByDay>>("title/schedule", null, forceRefresh);

        await Task.CompletedTask;
        return _scheduleData;
    }


    private Title _releaseData;
    public async Task<Title> GetTitleAsync(long id)
    {
        _releaseData = await _instance.GetAsync<Title>($"title?id={id}");

        await Task.CompletedTask;
        return _releaseData;
    }

    public async Task<TitlesSearchResult> SearchTitles(string queryString)
    {
        return await _instance.GetAsync<TitlesSearchResult>($"/title/search?search={queryString}&order_by=names.ru");
    }

    public async Task<UserData> GetUserAsync(string session)
    {
        return await _instance.GetAsync<UserData>($"user?session={session}");
    }
}
