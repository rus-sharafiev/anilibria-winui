using Anilibria.Core.Models;

namespace Anilibria.Contracts.Services;

public interface IVideoQualitySelectorService
{
    QltString Qlt
    {
        get;
    }

    Task InitializeAsync();

    Task SetVideoQualityAsync(QltString qlt);

    event EventHandler? QualityChanged;
}
