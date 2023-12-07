using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Anilibria.Converters;

public class ImageSourceConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var imagePath = (string)value;
        return new Uri(new Uri("https://static.wwnd.space"), imagePath);
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, string language)
    {
        throw new NotImplementedException();
    }

    #endregion
}
