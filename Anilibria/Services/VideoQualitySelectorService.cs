using Anilibria.Contracts.Services;
using Anilibria.Core.Models;

namespace Anilibria.Services;

public class VideoQualitySelectorService : IVideoQualitySelectorService
{
    private const string SettingsKey = "VideoQlt";

    public event EventHandler? QualityChanged;

    public QltString Qlt { get; set; } = QltString.SD;

    private readonly ILocalSettingsService _localSettingsService;

    public VideoQualitySelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        Qlt = await LoadVideoQualityFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetVideoQualityAsync(QltString qlt)
    {
        Qlt = qlt;

        QualityChanged?.Invoke(this, new EventArgs());
        await SaveVideoQualityInSettingsAsync(Qlt);
    }

    private async Task<QltString> LoadVideoQualityFromSettingsAsync()
    {
        var qltName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(qltName, out QltString cacheQlt))
        {
            return cacheQlt;
        }

        return QltString.SD;
    }

    private async Task SaveVideoQualityInSettingsAsync(QltString qlt)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, qlt.ToString());
    }
}
