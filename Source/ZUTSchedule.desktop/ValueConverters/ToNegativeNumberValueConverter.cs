using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZUTSchedule.desktop
{
    public class ToNegativeNumberValueConverter : BaseValueConverter<ToNegativeNumberValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value is double width)
            {
                return new Thickness(width, 0, -width, 0);
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
