using Anilibria.Core.Models;

namespace Anilibria.Core.Contracts.Services;

public interface IApiService
{
    Task<List<TitlesByDay>> GetScheduleAsync();

    Task<Title> GetTitleAsync(long id);
}
