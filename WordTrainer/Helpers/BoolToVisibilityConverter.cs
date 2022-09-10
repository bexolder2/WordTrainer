using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WordTrainer.Helpers
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;
            if (value != null && value is bool val)
            {
                visibility = val ? Visibility.Visible : Visibility.Collapsed;
                if (parameter != null && parameter.ToString() == "invert")
                {
                    visibility = val ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = false;
            if (value != null && value is Visibility visibility)
            {
                val = visibility == Visibility.Visible ? true : false;
                if (parameter != null && parameter.ToString() == "invert")
                {
                    val = visibility == Visibility.Visible ? false : true;
                }
            }

            return val;
        }
    }
}
