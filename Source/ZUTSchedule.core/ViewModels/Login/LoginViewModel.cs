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
        /// Login provided by user
        /// </summary>
        public string UserLogin { get; set; }

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
            EDziekanatService service = new EDziekanatService();

            // Get courses
            Storage.Classes = await service.getClasses(new List<DateTime>() { DateTime.Now });

            // Switch page to week view
            MainWindowViewModel.Instance.State = MainWindowState.WeekView;

        }


    }
}
