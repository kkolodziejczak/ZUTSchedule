using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZUTSchedule.core
{

    /// <summary>
    /// Class that stores users credentials and application settings
    /// </summary>
    public class Storage
    {

        public readonly string loginURL = "https://www.zut.edu.pl/WU/Logowanie2.aspx";
        public readonly string scheduleURL = "https://www.zut.edu.pl/WU/PodzGodzin.aspx";
        public readonly string LevelURL = "https://www.zut.edu.pl/WU/KierunkiStudiow.aspx";
        public readonly string logOutURL = "https://www.zut.edu.pl/WU/Wyloguj.aspx";

        public readonly string newsZutURL = "http://www.zut.edu.pl/zut-studenci/start/aktualnosci.html";
        public readonly string newsWiZutURL = "https://www.wi.zut.edu.pl/index.php/pl/dla-studenta/sprawy-studenckie/aktualnosci-studenckie";

        public static readonly string SettingsFolderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ZUTSchedule";
        public static readonly string SettingsFilePath = $@"{SettingsFolderPath}\Settings.ini";

        private Credential _userCredential;

        /// <summary>
        /// User Credentials
        /// </summary>
        [XmlIgnore]
        public Credential UserCredential
        {
            get
            {
                if (_userCredential != null)
                {
                    return _userCredential;
                }
                else
                {
                    return IoC.Get<ICredentialManager>().ReadCredential("ZUTSchedule");
                }
            }
            set => _userCredential = value;
        }

        /// <summary>
        /// Indicates if User is logged in to e-Dziekanat system
        /// </summary>
        public bool IsUserLoggedIn { get; set; }

        /// <summary>
        /// Mode in with user wants to login in 
        /// </summary>
        public string LoginAs { get; set; } = "student";

        /// <summary>
        /// Defines maximum length of the message;
        /// </summary>
        public int HowLongNewsMessages { get; set; }

        /// <summary>
        /// How many days NEW box will show next to news
        /// </summary>
        public int HowManyDaysIsNew { get; set; }

        /// <summary>
        /// Downloaded classes 
        /// </summary>
        public List<DayViewModel> Classes { get; set; }

        /// <summary>
        /// Number of days in the week to display
        /// </summary>
        public int NumberOfDaysInTheWeek { get; set; }

        /// <summary>
        /// Number of shifted days
        /// </summary>
        public int DayShift { get; set; } = 0;

        /// <summary>
        /// Reload classes from e-Dziekanat service
        /// </summary>
        public event Action OnRefresh = () => { };

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
            if (DayShift - 1 < 0)
            {
                return;
            }

            DayShift--;
            OnDayShiftUpdate();
        }

        /// <summary>
        /// Refreshes schedule with fresh data
        /// </summary>
        public void RefreshSchedule()
        {
            OnRefresh();
        }

        /// <summary>
        /// Copy all information from <paramref name="storage"/>
        /// </summary>
        /// <param name="storage"></param>
        public void CopyFrom(Storage storage)
        {
            this.Classes = storage.Classes;
            this.HowLongNewsMessages = storage.HowLongNewsMessages;
            this.HowManyDaysIsNew = storage.HowManyDaysIsNew;
            this.NumberOfDaysInTheWeek = storage.NumberOfDaysInTheWeek;
            this.LoginAs = storage.LoginAs;
            this.IsUserLoggedIn = storage.IsUserLoggedIn;
        }
    }
}
