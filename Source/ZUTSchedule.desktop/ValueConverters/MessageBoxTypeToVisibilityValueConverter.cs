using System;
using System.Globalization;
using System.Windows;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Converts <see cref="MessageBoxType"/> into <see cref="Visibility"/>
    /// </summary>
    public class MessageBoxTypeToVisibilityValueConverter : BaseValueConverter<MessageBoxTypeToVisibilityValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is MessageBoxType type))
                return null;

            if (type == MessageBoxType.Ok)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
