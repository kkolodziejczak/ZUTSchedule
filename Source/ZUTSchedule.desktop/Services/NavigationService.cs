using System.Threading.Tasks;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToWeekPage() => MainWindowViewModel.State = MainWindowState.WeekView;

        public async Task NavigateToLoginPage() => MainWindowViewModel.State = MainWindowState.LoginPage;

        public async Task NavigateToProgressIndicator() => MainWindowViewModel.State = MainWindowState.ProgressIndicator;

    }
}
