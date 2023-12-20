using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;
using System.Net;

namespace Anilibria.Core.Services;

public class ApiService : IApiService
{
    private readonly HttpDataService _instance = new();
    private readonly string _baseUrl = "https://wwnd.space";
    private readonly string _apiUrl = "https://wwnd.space/public/api/index.php";
    private readonly string _loginUrl = "https://wwnd.space/public/login.php";

    public ApiService()
    {
    }

    public async Task<Day[]> GetScheduleAsync(bool forceRefresh = false)
    {
        var formList = new List<KeyValuePair<string, string>>
        {
            new("query", "schedule"),
        };

        var response = await _instance.PostAsFormAsync<ScheduleBase>(_apiUrl, new FormUrlEncodedContent(formList), "schedule", forceRefresh);

        await Task.CompletedTask;
        return response.Data;
    }

    public async Task<Release> GetReleaseAsync(long id)
    {
        var formList = new List<KeyValuePair<string, string>>
        {
            new("query", "release"),
            new("id", $"{id}"),
        };

        var response = await _instance.PostAsFormAsync<ReleaseBase>(_apiUrl, new FormUrlEncodedContent(formList), $"release-{id}");

        await Task.CompletedTask;
        return response.Data;
    }

    public async Task<Release[]> SearchAsync(string queryString)
    {
        var formList = new List<KeyValuePair<string, string>>
        {
            new("query", "search"),
            new("search", queryString),
        };

        var response = await _instance.PostAsFormAsync<SearchBase>(_apiUrl, new FormUrlEncodedContent(formList), $"search-{queryString}");
        await Task.CompletedTask;
        return response.Data;
    }

    public async Task<UserData> GetUserAsync(string sessionId)
    {
        var formList = new List<KeyValuePair<string, string>>
        {
            new("query", "user"),
        };

        _instance.CookieContainer.Add(new Uri(_baseUrl), new Cookie("PHPSESSID", sessionId));
        var response = await _instance.PostAsFormAsync<UserBase>(_apiUrl, new FormUrlEncodedContent(formList), "user", true);
        await Task.CompletedTask;
        return response.Data;
    }

    public async Task<Session> GetUserSession(FormUrlEncodedContent form)
    {
        return await _instance.PostAsFormAsync<Session>(_loginUrl, form, "", true);
    }

    public CookieCollection GetCookies() =>
        _instance.GetCookieCollection().GetCookies(new Uri("https://wwnd.space"));

    public void RemoveCookies() =>
        _instance.CookieContainer.Add(new Uri(_baseUrl), new Cookie("PHPSESSID", string.Empty));
}
