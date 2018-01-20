using ZUTSchedule.core;
using System.Threading.Tasks;

namespace ZUTSchedule.mobile
{
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// Navigates to root Page
        /// </summary>
        /// <returns></returns>
        public Task GoToRootPage() => App.Page.Navigation.PopToRootAsync();

        /// <summary>
        /// Navigate To DayView Page
        /// </summary>
        /// <returns></returns>
        public Task NavigateToWeekPage() => App.Page.Navigation.PushAsync(new DayPage());

        /// <summary>
        /// Navigate To Login Page
        /// </summary>
        /// <returns></returns>
        public Task NavigateToLoginPage() => App.Page.Navigation.PushAsync(new LoginPage());

        public Task NavigateToProgressIndicator()
        {
            throw new System.NotImplementedException();
        }
    }
}
