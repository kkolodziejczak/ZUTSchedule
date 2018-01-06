using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reflection;

namespace ZUTSchedule.core
{
    public class LoginViewModel : BaseViewModel
    {

        private bool _isLogginIn;

        /// <summary>
        /// Login provided by user
        /// </summary>
        public string UserLogin
        {
            get { return IoC.Settings.login; }
            set { IoC.Settings.login = value; }
        }

        /// <summary>
        /// Password provided by user
        /// </summary>
        public SecureString UserPassword
        {
            get { return IoC.Settings.Password; }
            set { IoC.Settings.Password = value; }
        }

        /// <summary>
        /// Indicates if user is login in as teacher
        /// </summary>
        public bool IsTeacher
        {
            get { return IoC.Settings.Typ == "dydaktyk" ? true : false; }
            set { IoC.Settings.Typ = value ? "dydaktyk" : "student"; }
        }
        
        /// <summary>
        /// Indicates if login is in process
        /// </summary>
        public bool IsLogginIn
        {
            get => _isLogginIn;

            set
            {
                _isLogginIn = value;
                OnPropertyChanged(nameof(IsLogginIn));
            }
        }

        /// <summary>
        /// Indicates in with mode run application
        /// </summary>
        public int DayMode { get; set; } = 1;

        /// <summary>
        /// Command that login user into system
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Base constructor
        /// </summary>
        public LoginViewModel()
        {
            // setup commands
            LoginCommand = new RelayCommand(async() => await Login());
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            if(IsLogginIn == true)
            {
                return;
            }

            var service = IoC.EDziekanatService;

            // In case of something is missing
            if(string.IsNullOrWhiteSpace(UserLogin) || UserPassword.Length <= 0)
            {
                //TODO: signalize to login
                //TODO: Add MessageService
                return;
            }

            IsLogginIn = true;

            // Get courses
            IoC.Settings.Classes = await service.GetClasses(new List<DateTime>() { DateTime.Now });

            if(IoC.Settings.Classes.Count == 0)
            {
                IsLogginIn = false;
                return;
            }

            IoC.Settings.NumberOfDaysInTheWeek = DayMode == 0 ? 1 : DayMode == 1 ? 5 : 7;

            // Switch page to week view
            await IoC.Navigation.NavigateToWeekPage();

            IsLogginIn = false;
        }


    }
}
