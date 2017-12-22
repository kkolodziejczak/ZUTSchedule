using System.Threading.Tasks;


namespace ZUTSchedule.core
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to Login Page
        /// </summary>
        /// <returns></returns>
        Task NavigateToLoginPage();

        /// <summary>
        /// Navigates to Day Page
        /// </summary>
        /// <returns></returns>
        Task NavigateToWeekPage();

        /// <summary>
        /// Navigates to root Page
        /// </summary>
        /// <returns></returns>
        Task GoToRootPage();
    }
}