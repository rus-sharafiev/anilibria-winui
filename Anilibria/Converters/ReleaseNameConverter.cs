using Microsoft.UI.Xaml.Data;
using System.Globalization;

namespace Anilibria.Converters;

internal class ReleaseNameConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var names = (string[])value;
        var uiCulture = CultureInfo.CurrentUICulture;

        if (names.Length > 1)
            return uiCulture.Name switch 
            {
                "ru-RU" => names[0],
                "en-US" => names[1],
                _ => names[0],
            };
        else
            return names.First();
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, string language)
    {
        throw new NotImplementedException();
    }

    #endregion
}