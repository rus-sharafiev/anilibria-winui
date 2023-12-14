using System.Net.Http.Headers;
using System.Text;
using Anilibria.Core.Contracts.Services;
using Newtonsoft.Json;

namespace Anilibria.Core.Services;
public class HttpDataService : IHttpDataService
{
    private readonly Dictionary<string, object> responseCache;
    private readonly HttpClient client;

    public HttpDataService(string defaultBaseUrl = "")
    {
        client = new HttpClient();

        if (!string.IsNullOrEmpty(defaultBaseUrl))
        {
            client.BaseAddress = new Uri($"{defaultBaseUrl}/");
        }

        responseCache = new Dictionary<string, object>();
    }

    public async Task<T> GetAsync<T>(string uri, string accessToken = null, bool forceRefresh = false)
    {
        T result = default;

        if (forceRefresh || !responseCache.TryGetValue(uri, out var value))
        {
            System.Diagnostics.Debug.WriteLine("request");
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
            System.Diagnostics.Debug.WriteLine("from cache");
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

    public async Task<T> PostAsFormEncodeAsync<T>(string uri, List<KeyValuePair<string, string>> list)
    {
        var response = await client.PostAsync(uri, new FormUrlEncodedContent(list));
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        return await Task.Run(() => JsonConvert.DeserializeObject<T>(responseBody));
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
