using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZUTSchedule.core
{
    public class LoginViewModel : BaseViewModel
    {
        /// <summary>
        /// Service that allows to switch between pages
        /// </summary>
        private INavigationService _NavigationService;
        
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
        public string UserPassword
        {
            get { return Storage.Password; }
            set { Storage.Password = value; }
        }

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
        public LoginViewModel(INavigationService service)
        {
            // Get injected navigation service
            _NavigationService = service;

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
            EDziekanatService service = new EDziekanatService();

            // In case of something is missing
            if(string.IsNullOrWhiteSpace(UserLogin) || string.IsNullOrWhiteSpace(UserPassword))
            {
                //TODO: signalize to login
                //TODO: Add MessageService
                return;
            }

            // Get courses
            Storage.Classes = await service.getClasses(new List<DateTime>() { DateTime.Now });

            // Switch page to week view
            MainWindowViewModel.Instance.State = MainWindowState.WeekView;

            await _NavigationService.NavigateToDayPage();

        }


    }
}
