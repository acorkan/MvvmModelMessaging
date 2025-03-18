using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MileHighWpf.MvvmModelMessagingDemo.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            bool boolValue = (value != null) ? (bool)value : false;
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null; //throw new NotSupportedException();
        }
    }
}
