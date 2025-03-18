using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MileHighWpf.MvvmModelMessagingDemo.Converters
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (value != null) ? (bool)value : false;
            boolValue = (parameter != null) ? !boolValue : boolValue;
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
