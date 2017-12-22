using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public enum MainWindowState
    {
        loginPage,
        WeekView,
    }

    public class MainWindowViewModel : BaseViewModel
    {
        private MainWindowState _State;

        /// <summary>
        /// State in with MainWindow is
        /// </summary>
        public MainWindowState State
        {
            get { return _State; }
            set
            {
                if (_State == value)
                    return;

                _State = value;

                OnPropertyChanged(nameof(State));
            }
        }

        /// <summary>
        /// Current version of the application
        /// </summary>
        public static string AppVersion { get; private set; }

        /// <summary>
        /// Base constructor
        /// </summary>
        public MainWindowViewModel()
        {
            State = MainWindowState.loginPage;
            AppVersion = $"ZUTSchedule v{Assembly.GetEntryAssembly().GetName().Version.ToString()}";
        }

    }
}
