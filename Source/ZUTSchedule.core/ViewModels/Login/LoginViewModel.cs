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
        
        /// <summary>
        /// Login provided by user
        /// </summary>
        public string UserLogin
        {
            get { return Storage.login; }
            set { Storage.login = value; }
        }

        /// <summary>
        /// Password provided by user
        /// </summary>
        public SecureString UserPassword
        {
            get { return Storage.Password; }
            set { Storage.Password = value; }
        }

        /// <summary>
        /// Version of the application
        /// </summary>
        public string AppVersionString { get => MainWindowViewModel.AppVersion; }

        /// <summary>
        /// Indicates if user is login in as teacher
        /// </summary>
        public bool IsTeacher
        {
            get { return Storage.Typ == "dydaktyk" ? true : false; }
            set { Storage.Typ = value ? "dydaktyk" : "student"; }
        }

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
            // Create new Service
            EDziekanatService EDziekanatService = new EDziekanatService();

            // In case of something is missing
            if(string.IsNullOrWhiteSpace(UserLogin) || UserPassword.Length <= 0)
            {
                //TODO: signalize to login
                //TODO: Add MessageService
                return;
            }

            // Get courses
            Storage.Classes = await EDziekanatService.getClasses(new List<DateTime>() { DateTime.Now });

            // Switch page to week view
            await IoC.Get<INavigationService>().NavigateToWeekPage();

        }


    }
}
