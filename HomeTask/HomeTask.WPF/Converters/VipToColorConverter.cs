using System;
using System.Windows.Data;
using System.Windows.Media;

namespace HomeTask.WPF.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class VipToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return Brushes.LightPink;
            return Brushes.WhiteSmoke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
