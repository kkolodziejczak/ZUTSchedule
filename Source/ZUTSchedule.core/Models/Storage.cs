using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace ZUTSchedule.core
{

    /// <summary>
    /// Class that stores users credentials and application settings
    /// </summary>
    public class Storage
    {

        public readonly string loginURL = "https://www.zut.edu.pl/WU/Logowanie2.aspx";
        public readonly string scheduleURL = "https://www.zut.edu.pl/WU/PodzGodzin.aspx";
        public readonly string logOutURL = "https://www.zut.edu.pl/WU/Wyloguj.aspx";

        public readonly string newsZutURL = "http://www.zut.edu.pl/zut-studenci/start/aktualnosci.html#";
        public readonly string newsWiZutURL = "https://www.wi.zut.edu.pl/index.php/pl/dla-studenta/sprawy-studenckie/aktualnosci-studenckie";

        public static readonly string SettingsFolderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ZUTSchedule";

        public static readonly string SettingsFilePath = $@"{SettingsFolderPath}\Settings.ini";

        // User Credentials 
        public string login;
        public SecureString Password;
        public string Typ;

        /// <summary>
        /// Defines maximum length of the message;
        /// </summary>
        public int HowLongNewsMessages { get; } = 50;

        /// <summary>
        /// How many days NEW box will show next to news
        /// </summary>
        public int HowManyDaysIsNew { get; } = 3;

        /// <summary>
        /// Downloaded classes 
        /// </summary>
        public List<DayViewModel> Classes { get; set; }

        /// <summary>
        /// Number of days in the week to display
        /// </summary>
        public int NumberOfDaysInTheWeek { get; set; } = 5;

        /// <summary>
        /// Number of shifted days
        /// </summary>
        public int DayShift { get; set; } = 0;

        /// <summary>
        /// Fired when week is changing 
        /// </summary>
        public event Action OnDayShiftUpdate = () => { };

        /// <summary>
        /// Switches to next week
        /// </summary>
        public void IncrementWeek()
        {
            DayShift++;
            OnDayShiftUpdate();
        }

        /// <summary>
        /// Switches to week earlier 
        /// </summary>
        public void DecrementWeek()
        {
            if(DayShift - 1 < 0)
            {
                return;
            }

            DayShift--;
            OnDayShiftUpdate();
        }

        /// <summary>
        /// Refreshes schedule
        /// </summary>
        public void RefreshSchedule()
        {
            OnDayShiftUpdate();
        }
    }
}
