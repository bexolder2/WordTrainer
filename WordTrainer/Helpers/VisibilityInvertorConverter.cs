using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WordTrainer.Helpers
{
    public class VisibilityInvertorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;
            if (value != null && value is Visibility)
            {
                result = (Visibility)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
