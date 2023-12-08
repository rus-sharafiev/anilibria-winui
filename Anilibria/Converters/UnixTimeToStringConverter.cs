using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anilibria.Core.Models;
using Microsoft.UI.Xaml.Data;

namespace Anilibria.Converters;
public class UnixTimeToStringConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var seconds = (long)value;
        return DateTimeOffset.FromUnixTimeSeconds(seconds).ToString("G");
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, string language)
    {
        throw new NotImplementedException();
    }

    #endregion
}