using Anilibria.Contracts.Services;

namespace Anilibria.Services;

internal class ImageService : IImageService
{
    private const string _baseUri = "https://static.wwnd.space";
    private const string _imageCacheFolderPathName = "Anilibria/ApplicationData/CachedImages";

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _imageCacheFolder;

    private bool _isInitialized;

    private List<string> _imagesList;
    private readonly HttpClient client = new();

    public ImageService()
    {
        _imageCacheFolder = Path.Combine(_localApplicationData, _imageCacheFolderPathName);
        _imagesList = [];
    }

    public async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            if (Directory.Exists(_imageCacheFolder))
                _imagesList = await Task.Run(() => Directory.GetFiles(_imageCacheFolder).ToList() ?? []);
            else
                Directory.CreateDirectory(_imageCacheFolder);

            _isInitialized = true;
        }
    }

    public bool CacheContains(string imagePath) {
        var filePath = Path.Combine(_imageCacheFolder, Path.GetFileName(imagePath));
        return _imagesList.Contains(filePath);
    } 

    public Uri GetCachedImageUri(string imagePath)
    {
        var filePath = Path.Combine(_imageCacheFolder, Path.GetFileName(imagePath));
        return new Uri(filePath, UriKind.Absolute);
    }

    public async Task CacheImageAsync(string imagePath)
    {
        await InitializeAsync();

        var filePath = Path.Combine(_imageCacheFolder, Path.GetFileName(imagePath));

        var stream = await client.GetStreamAsync(_baseUri + imagePath);
        var fileStream = new FileStream(filePath, FileMode.CreateNew);
        await stream.CopyToAsync(fileStream);

        _imagesList.Add(filePath);
    }
}
