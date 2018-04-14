using Simple.Xamarin.Framework.core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZUTSchedule.core;

namespace ZUTSchedule.mobile
{

    /// <summary>
    /// LoginPage ViewModel that tells Page what to display
    /// </summary>
    public class LoginPageViewModel : BasePageViewModel
    {
        /// <summary>
        /// ViewModel with logic
        /// </summary>
        public LoginViewModel LoginVM { get; set; }

        /// <summary>
        /// Base Constructor
        /// </summary>
        public LoginPageViewModel()
        {
            LoginVM = new LoginViewModel();
            LoginVM.SetLoginAsStudentCommand.Execute(null);

        }

        public override void InitializeNavigationBar()
        {
            NavigationBar = new NavigationBarViewModel
            {
                Title = "Login",
            };
            NavigationBar.Show();
        }

        public override void InitializeUpperToolBar()
        {
        }

        public override void InitializeBottomToolBar()
        {
        }

    }
}
