using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Helper class that allows to perform additional actions on static resources
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Returns value from static resource
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public static object GetStaticFieldValue(string KeyValue)
        {
            object returnValue = null;

            try
            {
                returnValue = Application.Current.FindResource(KeyValue);
            }
            catch
            {
                // Ignore any errors and return null
            }

            return returnValue;
        }
    }
}
