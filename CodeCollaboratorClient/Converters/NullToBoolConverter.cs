using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodeCollaboratorClient.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null) ^ (parameter as string ?? string.Empty).Equals("Reverse", StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
