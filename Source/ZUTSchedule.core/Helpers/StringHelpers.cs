using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public static class StringHelpers
    {
        public static int ToInt(this string str)
        {
            return Int32.Parse(str);
        }
    }
}
