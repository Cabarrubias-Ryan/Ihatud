using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Ihatud.Converters
{
    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAccepted = (bool)value;
            return isAccepted ? "Wait in 20 mins your order will arrive" : "Waiting";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}