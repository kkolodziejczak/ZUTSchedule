﻿using System.Net.Http;
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
            get { return IoC.Settings.Type == "dydaktyk" ? true : false; }
            set { IoC.Settings.Type = (value ? "dydaktyk" : "student"); }
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
        /// Indicates if user wants to start application automatically 
        /// </summary>
        public bool AutoRunEnabled { get; set; }

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
            // Check for AutoRun
            AutoRunEnabled = IoC.Get<IAutoRun>().IsAutoRunEnabled();
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
                Logger.Warning("Username or Password are empty");
                //TODO: signalize to user
                //TODO: Add MessageService
                return;
            }
            else
            {
                IoC.Settings.UserCredential = new Credential(Credential.CredentialType.Generic,
                                                            "ZUTSchedule",
                                                            UserLogin,
                                                            UserPassword);
            }

            IsLoginProcessing = true;

            //Perform Login Attempt
            var loggedIn = await TryLoginAsync(IoC.Settings.UserCredential);

            if (loggedIn == false)
            {
                Logger.Warning("Login failed");
                IsLoginProcessing = false;
                //TODO: signalize to user

                return;
            }

            IoC.Settings.NumberOfDaysInTheWeek = (DayMode == 0) ? 1 : (DayMode == 1 ? 5 : 7);

            if (AutoLoginEnabled == true)
            {
                // Save users credentials for later use
                IoC.CredentialManager.SaveCredential("ZUTSchedule", UserLogin, UserPassword);
            }

            if (AutoRunEnabled == true)
            {
                // Enable auto run with Windows
                IoC.Get<IAutoRun>().EnableAutoRun();
            }
            else
            {
                // Disable auto run with Windows
                IoC.Get<IAutoRun>().DisableAutoRun();
            }

            // Logout no need to be logged in
            await businessLogic.LogoutAsync();

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
                await businessLogic.LoginAsync(credential);
                return IoC.Settings.IsUserLoggedIn;
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"Login failed! \n {ex.Message} \n\n {ex.StackTrace}");
                //TODO: signalize Fail login attempt With request // maybe connection issue
                return false;
            }
        }
    }
}
