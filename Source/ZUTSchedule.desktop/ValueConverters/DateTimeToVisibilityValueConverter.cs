using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZUTSchedule.desktop
{
    public class DateTimeToVisibilityValueConverter : BaseValueConverter<DateTimeToVisibilityValueConverter>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">!= null Left == null Right</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DateTime) && parameter == null)
                return null;

            var type = (DateTime)value;

            //Left
            if (parameter != null)
            {
                if (type.DayOfWeek == DayOfWeek.Monday)
                {
                    return Visibility.Visible;
                }
            }
            // Right
            else
            {
                if (type.DayOfWeek == DayOfWeek.Friday)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
