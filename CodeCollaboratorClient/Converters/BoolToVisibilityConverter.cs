using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodeCollaboratorClient.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value is bool boolValue)
            {
                return boolValue ^ (parameter as string ?? string.Empty).Equals("Reverse", StringComparison.OrdinalIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;
            }

            // value is not of type bool.
            throw new InvalidOperationException("Value must be a bool");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibilityValue)
            {
                return visibilityValue == Visibility.Visible ^ (parameter as string ?? string.Empty).Equals("Reverse", StringComparison.OrdinalIgnoreCase);
            }

            // value is not of type Visibility.
            throw new InvalidOperationException("Value must be a Visibility");
        }
    }
}
