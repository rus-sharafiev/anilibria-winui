using Anilibria.Core.Models;
using System.Net;

namespace Anilibria.Core.Contracts.Services;

public interface IApiService
{
    Task<Day[]> GetScheduleAsync(bool forceRefresh = false);

    Task<Release> GetReleaseAsync(long id);

    Task<Release[]> SearchAsync(string queryString);

    Task<UserData> GetUserAsync(string sessionId);

    Task<Session> GetUserSession(FormUrlEncodedContent form);

    CookieCollection GetCookies();
    void RemoveCookies();
}
