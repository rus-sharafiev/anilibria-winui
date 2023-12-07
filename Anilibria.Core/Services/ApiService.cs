using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;

namespace Anilibria.Core.Services;

public class ApiService : IApiService
{
    private readonly HttpDataService _instance = new("https://anilibria.srr.workers.dev");

    public ApiService()
    {
    }

    private List<TitlesByDay> _scheduleData;
    public async Task<List<TitlesByDay>> GetScheduleAsync()
    {
        _scheduleData = await _instance.GetAsync<List<TitlesByDay>>("title/schedule");

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
}
