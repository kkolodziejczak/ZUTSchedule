using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZUTSchedule.desktop
{
    public class DateTimeToTodayValueConverter : BaseValueConverter<DateTimeToTodayValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DateTime date))
                return null;

            if(DateTime.Compare(date, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)) == 0)
            {
                return ResourceHelper.GetStaticFieldValue("ZUTBlueColorBrush");
            }


            return ResourceHelper.GetStaticFieldValue("ZUTGreenColorBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
