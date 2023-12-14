using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Anilibria.Core.Contracts.Services;
public interface IHttpDataService
{
    Task<T> GetAsync<T>(string uri, string accessToken = null, bool forceRefresh = false);

    Task<bool> PostAsync<T>(string uri, T item);
    Task<bool> PostAsJsonAsync<T>(string uri, T item);
    Task<T> PostAsFormEncodeAsync<T>(string uri, List<KeyValuePair<string, string>> list);

    Task<bool> PutAsync<T>(string uri, T item);
    Task<bool> PutAsJsonAsync<T>(string uri, T item);

    Task<bool> DeleteAsync(string uri);
}
