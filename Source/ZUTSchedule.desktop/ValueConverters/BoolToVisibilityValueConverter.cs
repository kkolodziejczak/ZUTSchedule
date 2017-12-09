using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZUTSchedule.desktop
{
    public class BoolToVisibilityValueConverter : BaseValueConverter<BoolToVisibilityValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool type))
                return null;

            bool ConverterLogic = false;

            if (parameter != null)
            {
                // negate converter logic if parameter is provided
                ConverterLogic = !ConverterLogic;
            }

            if (type == ConverterLogic)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;

            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
