using Anilibria.Contracts.Services;
using Microsoft.UI.Xaml.Data;

namespace Anilibria.Converters;

public class ImageSourceConverter : IValueConverter
{
    #region IValueConverter Members

    private const string _baseUri = "https://static.wwnd.space";
    private readonly IImageService _imageService = App.GetService<IImageService>();

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var imagePath = (string)value;

        if (_imageService.CacheContains(imagePath))
        {
            return _imageService.GetCachedImageUri(imagePath);
        }
        else
        {
            _imageService.CacheImageAsync(imagePath);
            return new Uri(_baseUri + imagePath);
        }
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, string language)
    {
        throw new NotImplementedException();
    }

    #endregion
}
