using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Anilibria.Core.Contracts.Services;
using Newtonsoft.Json;

namespace Anilibria.Core.Services;
public class HttpDataService : IHttpDataService
{
    private readonly Dictionary<string, object> responseCache;
    private readonly HttpClient client;
    private readonly HttpClientHandler handler;
    public CookieContainer CookieContainer { get; set; } = new();

    public HttpDataService(string defaultBaseUrl = "")
    {
        handler = new HttpClientHandler() { CookieContainer = CookieContainer };
        client = new HttpClient(handler);

        if (!string.IsNullOrEmpty(defaultBaseUrl))
        {
            client.BaseAddress = new Uri($"{defaultBaseUrl}/");
        }

        responseCache = [];
    }


    public async Task<T> PostAsFormAsync<T>(string uri, FormUrlEncodedContent content, string contentKey, bool forceRefresh = false)
    {
        T result = default;

        if (forceRefresh || !responseCache.TryGetValue(contentKey, out var value))
        {
            var response = await client.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine("request");
            result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseBody));

            if (responseCache.ContainsKey(contentKey))
            {
                responseCache[contentKey] = result;
            }
            else
            {
                responseCache.Add(contentKey, result);
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("from cache");
            result = (T)value;
        }

        return result;
    }

    // --------------------------------------------------------------------------------

    public async Task<T> GetAsync<T>(string uri, string accessToken = null, bool forceRefresh = false)
    {
        T result = default;

        if (forceRefresh || !responseCache.TryGetValue(uri, out var value))
        {
            AddAuthorizationHeader(accessToken);
            var json = await client.GetStringAsync(uri);
            result = await Task.Run(() => JsonConvert.DeserializeObject<T>(json));

            if (responseCache.ContainsKey(uri))
            {
                responseCache[uri] = result;
            }
            else
            {
                responseCache.Add(uri, result);
            }
        }
        else
        {
            result = (T)value;
        }

        return result;
    }

    public async Task<bool> PostAsync<T>(string uri, T item)
    {
        if (item == null)
        {
            return false;
        }

        var serializedItem = JsonConvert.SerializeObject(item);
        var buffer = Encoding.UTF8.GetBytes(serializedItem);
        var byteContent = new ByteArrayContent(buffer);

        var response = await client.PostAsync(uri, byteContent);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PostAsJsonAsync<T>(string uri, T item)
    {
        if (item == null)
        {
            return false;
        }

        var serializedItem = JsonConvert.SerializeObject(item);

        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync<T>(string uri, T item)
    {
        if (item == null)
        {
            return false;
        }

        var serializedItem = JsonConvert.SerializeObject(item);
        var buffer = Encoding.UTF8.GetBytes(serializedItem);
        var byteContent = new ByteArrayContent(buffer);

        var response = await client.PutAsync(uri, byteContent);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsJsonAsync<T>(string uri, T item)
    {
        if (item == null)
        {
            return false;
        }

        var serializedItem = JsonConvert.SerializeObject(item);

        var response = await client.PutAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string uri)
    {
        var response = await client.DeleteAsync(uri);

        return response.IsSuccessStatusCode;
    }

    public CookieContainer GetCookieCollection() => handler.CookieContainer;
    public void RemoveCookies(Uri uri) => handler.CookieContainer.Add(uri, []);

    // Add this to all public methods
    private void AddAuthorizationHeader(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = null;
            return;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
