using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZUTSchedule.core
{
    public class LoginViewModel : BaseViewModel
    {

        private bool _isLogginIn;

        /// <summary>
        /// Login provided by user
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Password provided by user
        /// </summary>
        public SecureString UserPassword { get; set; }

        /// <summary>
        /// Indicates if user is login in as teacher
        /// </summary>
        public bool IsTeacher
        {
            get { return IoC.Settings.Typ == "dydaktyk" ? true : false; }
            set { IoC.Settings.Typ = (value ? "dydaktyk" : "student"); }
        }

        /// <summary>
        /// Indicates if login is in process
        /// </summary>
        public bool IsLoginProcessing
        {
            get => _isLogginIn;

            set
            {
                _isLogginIn = value;
                OnPropertyChanged(nameof(IsLoginProcessing));
            }
        }

        /// <summary>
        /// Indicates if user wants to login automatically 
        /// </summary>
        public bool AutoLoginEnabled { get; set; }

        /// <summary>
        /// Indicates in with mode run application
        /// </summary>
        public int DayMode { get; set; }

        /// <summary>
        /// Command that login user into system
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Base constructor
        /// </summary>
        public LoginViewModel(int dayMode = 1)
        {

            

            // 1 for 5 days
            DayMode = dayMode;
            // setup commands
            LoginCommand = new RelayCommand(async () => await Login());

        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            if (IsLoginProcessing == true)
            {
                return;
            }

            // In case of something is missing
            if (string.IsNullOrWhiteSpace(UserLogin) || UserPassword.Length <= 0)
            {
                Logger.Log("Username or Password are empty", Logger.LogLevel.Warning);
                //TODO: signalize to user
                //TODO: Add MessageService
                return;
            }

            IsLoginProcessing = true;

            //Perform Login Attempt
            var loggedIn = await TryLoginAsync(new Credential(Credential.CredentialType.Generic,
                                                            "ZUTSchedule",
                                                            UserLogin,
                                                            UserPassword));

            if (loggedIn == false)
            {
                Logger.Log("Login failed", Logger.LogLevel.Warning);
                //TODO: signalize to user

                return;
            }

            IoC.Settings.NumberOfDaysInTheWeek = (DayMode == 0) ? 1 : (DayMode == 1 ? 5 : 7);

            if (AutoLoginEnabled == true)
            {
                // Save users credentials for later use
                IoC.CredentialManager.SaveCredential("ZUTSchedule", UserLogin, UserPassword);
            }

            IsLoginProcessing = false;

            // Switch page to week view
            await IoC.Navigation.NavigateToWeekPage();
        }


        /// <summary>
        /// Login into e-Dziekanat site
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        private async Task<bool> TryLoginAsync(Credential credential)
        {
            try
            {
                return await businessLogic.LoginAsync(credential);
            }
            catch (HttpRequestException ex)
            {
                Logger.Log($"Login failed! \n {ex.Message} \n\n {ex.StackTrace}", Logger.LogLevel.Error);
                //TODO: signalize Fail login attempt With request // maybe connection issue
                return false;
            }
        }
    }
}
