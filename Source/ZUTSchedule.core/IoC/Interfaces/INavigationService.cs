using System.Threading.Tasks;


namespace ZUTSchedule.core
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to progress indicator page
        /// </summary>
        /// <returns></returns>
        Task NavigateToProgressIndicator();

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

    }
}