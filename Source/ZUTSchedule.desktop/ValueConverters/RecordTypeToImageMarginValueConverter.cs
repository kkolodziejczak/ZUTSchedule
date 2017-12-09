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
    public class RecordTypeToImageMarginValueConverter : BaseValueConverter<RecordTypeToImageMarginValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is NewsType type)
            {
                switch (type)
                {
                    case NewsType.Global:
                        return new Thickness(0,4,0,0);
                    case NewsType.Wi:
                        return new Thickness(2);
                }
            }
            return new Thickness(2);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
