using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace ZUTSchedule.core
{
    public static class MatchCollectionHelper
    {
        /// <summary>
        /// Returns Value of item at <paramref name="place"/> from Group number 1.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public static string getValueAt(this MatchCollection collection, int place)
        {
            return collection[place].Groups[1].Value;
        }


        /// <summary>
        /// Returns Simple <see cref="DateTime"/> from Match Collection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this MatchCollection collection, string time)
        {

            int ClassDay = Int32.Parse(collection[0].Groups[1].Value);
            int ClassMonth = Int32.Parse(collection[0].Groups[2].Value);
            int ClassYear = Int32.Parse(collection[0].Groups[3].Value);

            var times = time.Split(':');
            int Hour = Int32.Parse(times[0]);
            int Mins = Int32.Parse(times[1]);

            return new DateTime(ClassYear, ClassMonth, ClassDay,Hour,Mins,0);

        }
    }
}
