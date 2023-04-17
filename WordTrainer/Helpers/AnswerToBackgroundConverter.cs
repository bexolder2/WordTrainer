using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WordTrainer.Models.Enums;

namespace WordTrainer.Helpers
{
    public class AnswerToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = Application.Current.Resources["PrimaryHueDarkBrush"] as SolidColorBrush;
            if (value != null && value is AnswerStatus status)
            {
                brush = status switch 
                {
                    AnswerStatus.None => Application.Current.Resources["PrimaryHueDarkBrush"] as SolidColorBrush,
                    AnswerStatus.Correct => Application.Current.Resources["RightAnswerBrush"] as SolidColorBrush,
                    AnswerStatus.Wrong => Application.Current.Resources["WrongAnswerBrush"] as SolidColorBrush
                };
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
