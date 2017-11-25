using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public static class DateTimeHelper
    {
        public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek)
        {
            int diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Returns if classes are happening in this hour
        /// </summary>
        /// <param name="date"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool IsRightNow(this DateTime date)
        {
            return date.Hour  >= DateTime.Now.Hour
                && date.Hour  <= DateTime.Now.Hour + 2
                && date.Year  == DateTime.Now.Year
                && date.Month == DateTime.Now.Month
                && date.Day   == DateTime.Now.Day;
        }
    }
}
