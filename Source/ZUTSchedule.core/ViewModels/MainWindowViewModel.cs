using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZUTSchedule.core
{
    public enum MainWindowState
    {
        loginPage,
        WeekView,
    }

    public class MainWindowViewModel : BaseViewModel
    {
        private MainWindowState _state;

        /// <summary>
        /// State in with MainWindow is
        /// </summary>
        public MainWindowState State
        {
            get { return _state; }
            set
            {
                if (_state == value)
                    return;

                _state = value;

                OnPropertyChanged(nameof(State));
            }
        }

        /// <summary>
        /// Base constructor
        /// </summary>
        public MainWindowViewModel()
        {
            State = MainWindowState.loginPage;
        }

    }
}
