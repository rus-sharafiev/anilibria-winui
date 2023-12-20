using Anilibria.Contracts.Services;
using Anilibria.Core.Contracts.Services;
using Anilibria.Core.Models;

namespace Anilibria.Services;

internal class UserService : IUserService
{
    private readonly IApiService _apiService;
    private readonly ILocalSettingsService _localSettingsService;
    private const string SettingsKey = "UserSession";
    private string? UserSession { get; set; }

    public UserData? User { get; set;}
    public event EventHandler? UserChanged;

    public LoginError? LoginError { get; set; }
    public event EventHandler? LoginErrorChanged;

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
                System.Diagnostics.Debug.WriteLine("User fetch error" + ex);
            }

        }
        await Task.CompletedTask;
    }

    public async void Login(string username, string password)
    {
        var formList = new List<KeyValuePair<string, string>>
        {
            new("mail", username),
            new("passwd", password)
        };
        var response = await _apiService.GetUserSession(new FormUrlEncodedContent(formList));

        if (response?.SessionId is not null)
        {
            UserSession = response.SessionId;
            await SaveUserSessionInSettingsAsync(response.SessionId);

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
        else
        {
            LoginError = new()
            {
                Key = response?.Key,
                Mes = response?.Mes,
            };
            LoginErrorChanged?.Invoke(this, new EventArgs());
        }
    }

    public async void LogOut()
    {
        User = null;
        UserSession = null;
        await RemoveUserSessionFromSettingsAsync();
        UserChanged?.Invoke(this, new EventArgs());
        _apiService.RemoveCookies();
    }

    private async Task<string?> LoadUserSessionFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
    }

    private async Task SaveUserSessionInSettingsAsync(string userSession)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, userSession);
    }

    private async Task RemoveUserSessionFromSettingsAsync()
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, string.Empty);
    }
}
