using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Mystic_ToDo.View.UserControls.Content.Calender.CalenderContent
{
    internal class YearToFontSizeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedYear && parameter is int year)
            {
                return selectedYear == year ? 24 : 12; // Adjust the font size as needed
            }
            return 12;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
