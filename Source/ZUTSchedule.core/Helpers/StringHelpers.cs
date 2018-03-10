using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public static class StringHelpers
    {
        /// <summary>
        /// Converts string to <see cref="Int32"/>
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="ArgumentNullException"
        /// <exception cref="FormatException"
        /// <exception cref="OverflowException"
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return Int32.Parse(str);
        }

        /// <summary>
        /// Returns true if strings are not the same
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toCompareAgainst"></param>
        /// <returns></returns>
        public static bool Is(this string value, string toCompareAgainst)
        {
            return 0 == string.CompareOrdinal(value, toCompareAgainst);
        }

        /// <summary>
        /// Returns true if strings are not the same
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toCompareAgainst"></param>
        /// <returns></returns>
        public static bool IsNot(this string value, string toCompareAgainst)
        {
            return !value.Is(toCompareAgainst);
        }

        /// <summary>
        /// Returns true if two strings are equal ignoring character case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toCompareAgainst"></param>
        /// <exception cref="ArgumentException"
        /// <returns></returns>
        public static bool IsIgnoreCase(this string value, string toCompareAgainst)
        {
            return string.Equals(value, toCompareAgainst, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns true if two strings are not equal ignoring character case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toCompareAgainst"></param>
        /// <exception cref="ArgumentException"
        /// <returns></returns>
        public static bool IsNotIgnoreCase(this string value, string toCompareAgainst)
        {
            return !value.IsIgnoreCase(toCompareAgainst);
        }

        /// <summary>
        /// Returns true if string is null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Returns true if string is not null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

    }
}
