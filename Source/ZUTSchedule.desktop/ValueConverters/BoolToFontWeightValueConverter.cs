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
    public class BoolToFontWeightValueConverter : BaseValueConverter<BoolToFontWeightValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool Now))
                return null;

            return Now ? FontWeights.Bold : FontWeights.Regular;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
