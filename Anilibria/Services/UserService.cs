using Anilibria.Contracts.Services;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;
using Anilibria.Core.Services;

namespace Anilibria.Services;

internal class UserService : IUserService
{
    private readonly IApiService _apiService;
    private readonly ILocalSettingsService _localSettingsService;
    private const string SettingsKey = "UserSession";
    private string? UserSession { get; set; }
    private readonly HttpDataService _httpDataService = new("https://login.anilibria.srrlab.ru");

    public UserData? User { get; set;}
    public event EventHandler? UserChanged;

    public UserService(ILocalSettingsService localSettingsService, IApiService apiService)
    {
        _apiService = apiService;
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        UserSession = await LoadUserSessionFromSettingsAsync();
        if (UserSession is not null)
        {
            try
            {
                User = await _apiService.GetUserAsync(UserSession);
                UserChanged?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

        }
        await Task.CompletedTask;
    }

    public async void Login(string username, string password)
    {
        var list = new List<KeyValuePair<string, string>>
        {
            new("mail", username),
            new("passwd", password),
        };

        var session = await _httpDataService.PostAsFormEncodeAsync<Session>("", list);

        if (session.SessionId is not null)
        {
            UserSession = session.SessionId;
            await SaveUserSessionInSettingsAsync(session.SessionId);

            try
            {
                User = await _apiService.GetUserAsync(session.SessionId);
                UserChanged?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }

    public void LogOut()
    {
        User = null;
        UserChanged?.Invoke(this, new EventArgs());
    }

    private async Task<string?> LoadUserSessionFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
    }

    private async Task SaveUserSessionInSettingsAsync(string userSession)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, userSession);
    }
}
