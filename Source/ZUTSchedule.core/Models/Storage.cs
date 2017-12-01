using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public static class Storage
    {
        // Temp Login data
        public static string login;
        public static string Password;
        public static string Typ;
        //

        public static int NumberOfDaysInTheWeek { get; } = 7;

        public static string LoginURL { get; } = "https://www.zut.edu.pl/WU/Logowanie2.aspx";
        public static string PlanURL { get; } = "https://www.zut.edu.pl/WU/PodzGodzin.aspx";
        public static string LogOutURL { get; } = "https://www.zut.edu.pl/WU/Wyloguj.aspx";

        public static List<DayViewModel> Classes { get; set; }

        public static List<DayOfWeek> WeekDays { get; } = new List<DayOfWeek>
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

    }
}
