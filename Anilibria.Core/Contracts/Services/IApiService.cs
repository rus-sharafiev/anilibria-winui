using Anilibria.Core.Models;

namespace Anilibria.Core.Contracts.Services;

public interface IApiService
{
    Task<List<TitlesByDay>> GetScheduleAsync(bool forceRefresh = false);

    Task<Title> GetTitleAsync(long id);

    Task<UserData> GetUserAsync(string session);

    Task<TitlesSearchResult> SearchTitles(string queryString);
}
