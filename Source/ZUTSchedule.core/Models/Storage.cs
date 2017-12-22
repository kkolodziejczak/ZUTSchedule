using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace ZUTSchedule.core
{
    public class Storage
    {
        // Temp Login data
        public static string login;
        public static SecureString Password;
        public static string Typ;
        //

        public static int NumberOfDaysInTheWeek { get; } = 1;
        public static int DayShift { get; set; } = 0;

        public delegate void ShiftDayUpdate();

        public event ShiftDayUpdate OnDayShiftUpdate = () => { };

        public void IncrementWeek()
        {
            DayShift++;
            OnDayShiftUpdate();
        }

        public void DecrementWeek()
        {
            DayShift--;
            OnDayShiftUpdate();
        }


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

        private static Storage _Instance;
        public static Storage Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new Storage();

                return _Instance;
            }
        }

        private Storage()
        {

        }
    }
}
