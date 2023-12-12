using Microsoft.UI.Xaml.Data;
using Microsoft.VisualBasic;

namespace Anilibria.Converters;

class StringListToStringConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var strings = (string[])value;
        return string.Join(", ", strings);
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, string language)
    {
        throw new NotImplementedException();
    }

    #endregion
}