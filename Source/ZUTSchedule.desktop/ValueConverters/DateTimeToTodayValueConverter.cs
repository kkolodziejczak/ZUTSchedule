using System;
using System.Globalization;

using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class BoolToTodayColorValueConverter : BaseValueConverter<BoolToTodayColorValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool Today))
                return null;

            return ResourceHelper.GetStaticFieldValue(Today ? "ZUTBlueColorBrush" : "ZUTGreenColorBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
