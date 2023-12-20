using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Anilibria.Core.Contracts.Services;
public interface IHttpDataService
{
    CookieContainer CookieContainer { get; set; }

    Task<T> GetAsync<T>(string uri, string accessToken = null, bool forceRefresh = false);

    Task<T> PostAsFormAsync<T>(string uri, FormUrlEncodedContent content, string contentKey, bool forceRefresh = false);
    Task<bool> PostAsync<T>(string uri, T item);
    Task<bool> PostAsJsonAsync<T>(string uri, T item);

    Task<bool> PutAsync<T>(string uri, T item);
    Task<bool> PutAsJsonAsync<T>(string uri, T item);

    Task<bool> DeleteAsync(string uri);

    CookieContainer GetCookieCollection();
    void RemoveCookies(Uri uri);
}
