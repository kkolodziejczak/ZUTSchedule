using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class MainWindowStateValueConverter : BaseValueConverter<MainWindowStateValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            switch ((MainWindowState)value)
            {
                case MainWindowState.loginPage:
                    return new LoginView();
                case MainWindowState.WeekView:
                    return new WeekView();
                default:
                    return null;

            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
