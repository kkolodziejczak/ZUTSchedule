using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class DateTimeToNowValueConverter : BaseValueConverter<DateTimeToNowValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DateTime date))
                return null;

            if(date.IsRightNow())
            {
                return FontWeights.Bold;
            }

            return FontWeights.Regular;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
