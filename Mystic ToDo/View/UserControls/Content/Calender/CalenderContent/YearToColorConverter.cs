using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Mystic_ToDo.View.UserControls.Content.Calender.CalenderContent
{
    internal class YearToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedYear && parameter is int year)
            {
                return selectedYear == year ? new SolidColorBrush(Color.FromRgb(199, 63, 105)) : new SolidColorBrush(Colors.Black);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
