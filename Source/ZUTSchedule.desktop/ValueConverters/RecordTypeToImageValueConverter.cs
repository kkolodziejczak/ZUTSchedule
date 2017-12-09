using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class RecordTypeToImageValueConverter : BaseValueConverter<RecordTypeToImageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value is NewsType type)
            {
                switch (type)
                {
                    case NewsType.Global:
                        return ResourceHelper.GetStaticFieldValue("ZUTDrawingImage");
                    case NewsType.Wi:
                        return ResourceHelper.GetStaticFieldValue("WILogoDrawingImage");
                }
            }
            return ResourceHelper.GetStaticFieldValue("ZUTDrawingImage"); ;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
