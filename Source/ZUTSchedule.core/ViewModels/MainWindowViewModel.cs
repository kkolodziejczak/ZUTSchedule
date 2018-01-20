using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public enum MainWindowState
    {
        LoginPage,
        WeekView,
        ProgressIndicator
    }

    public class MainWindowViewModel : BaseViewModel
    {
        private static MainWindowState _state;

        /// <summary>
        /// State in with MainWindow is
        /// </summary>
        public static MainWindowState State
        {
            get { return _state; }
            set
            {
                if (_state == value)
                    return;

                _state = value;

                OnStaticPropertyChanged();
            }
        }

        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public static event PropertyChangedEventHandler StaticPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="StaticPropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public static void OnStaticPropertyChanged([CallerMemberName]string name = null)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(name));
        }

        public MainWindowViewModel()
        {
            State = MainWindowState.ProgressIndicator;
            AttemtToLoginAutomaticly();
        }
        
        private async Task AttemtToLoginAutomaticly()
        {
            // Test if user stored he's credentials
            var credential = IoC.CredentialManager.ReadCredential("ZUTSchedule");

            // user have saved credentials...
            if (credential != null)
            {
                await IoC.Navigation.NavigateToProgressIndicator();

                // use them to login
                var loggedIn = false;

                try
                {
                    loggedIn = await businessLogic.LoginAsync(credential);
                }
                catch (HttpRequestException ex)
                {
                    // Do nothing
                }

                // login failed
                if (loggedIn == false)
                {
                    //TODO: signalize Fail login attempt
                    Logger.Log($"Automatic login failed!", Logger.LogLevel.Warning);
                    await IoC.Navigation.NavigateToLoginPage();
                    return;
                }

                await IoC.Navigation.NavigateToWeekPage();
                return;
            }

            await IoC.Navigation.NavigateToLoginPage();
        }

    }
}
