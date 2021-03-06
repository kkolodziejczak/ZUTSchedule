﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns <see cref="DateTime"/> with 1st day of the week starting from <paramref name="startOfWeek"/>
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startOfWeek">1st day of the week</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Returns true if <see cref="DateTime"/> is over week old
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsNotOlderThan(this DateTime date, int days)
        {
            var output = DateTime.Compare(date, DateTime.Now.AddDays(-days));

            return output <= 0; 
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with only Date values
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime OnlyDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// Converts a date to a week number.
        /// ISO 8601 week 1 is the week that contains the first Thursday that year.
        /// </summary>
        public static int ToIso8601Weeknumber(this DateTime date)
        {
            var thursday = date.AddDays(3 - date.DayOfWeek.DayOffset());
            return (thursday.DayOfYear - 1) / 7 + 1;
        }

        /// <summary>
        /// Iso8601 weeks start on Monday. This returns 0 for Monday.
        /// </summary>
        private static int DayOffset(this DayOfWeek weekDay)
        {
            return ((int)weekDay + 6) % 7;
        }

        private static Dictionary<DayOfWeek, string> Week = new Dictionary<DayOfWeek, string>()
        {
            {DayOfWeek.Monday, "Poniedziałek"},
            {DayOfWeek.Tuesday, "Wtorek"},
            {DayOfWeek.Wednesday, "Środa"},
            {DayOfWeek.Thursday, "Czwartek"},
            {DayOfWeek.Friday, "Piątek"},
            {DayOfWeek.Saturday, "Sobota"},
            {DayOfWeek.Sunday, "Niedziela"},
        };

        /// <summary>
        /// Returns DayOfWeek in Polish
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDayOfWeekPolish(this DateTime date)
        {
            return Week[date.DayOfWeek];
        }

        /// <summary>
        /// Returns number of days in <paramref name="year"/>
        /// </summary>
        /// <param name="date"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int DaysInTheYear(this DateTime date, int year)
        {
            return new DateTime(year, 12, 31).DayOfYear;
        }
    }
}
