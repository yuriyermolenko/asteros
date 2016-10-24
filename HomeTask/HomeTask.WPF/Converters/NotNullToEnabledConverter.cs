using System;
using System.Windows.Data;

namespace HomeTask.WPF.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class NotNullToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value != null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
