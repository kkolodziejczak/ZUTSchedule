using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.desktop
{
    public class ClassStatusToColorValueConverter :BaseValueConverter<ClassStatusToColorValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string Status))
                return null;

            switch (Status)
            {
                case "odwołane":
                    return ResourceHelper.GetStaticFieldValue("CanceledColorBrush");
                case "rektorskie":
                    return ResourceHelper.GetStaticFieldValue("RectorColorBrush");
                case "egzamin":
                    return ResourceHelper.GetStaticFieldValue("ExamColorBrush");
                default:
                    return ResourceHelper.GetStaticFieldValue("PrimaryTextBrush");
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
    }
}
