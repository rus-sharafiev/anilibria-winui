
namespace Anilibria.Contracts.Services;

public interface IImageService
{
    Task InitializeAsync();

    bool CacheContains(string imagePath);
    Uri GetCachedImageUri(string imagePath);
    Task CacheImageAsync(string imagePath);
}
