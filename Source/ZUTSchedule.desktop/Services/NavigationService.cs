using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    public class NavigationService : INavigationService
    {
        public async Task GoToRootPage() => IoC.Get<MainWindowViewModel>().State = MainWindowState.loginPage;

        public async Task NavigateToWeekPage() => IoC.Get<MainWindowViewModel>().State = MainWindowState.WeekView;

        public async Task NavigateToLoginPage() => IoC.Get<MainWindowViewModel>().State = MainWindowState.loginPage;

    }
}
