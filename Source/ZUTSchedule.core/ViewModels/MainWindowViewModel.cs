using System;
using System.Collections.Generic;
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

        private static MainWindowViewModel _Instance;

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
        /// Singleton
        /// </summary>
        public static MainWindowViewModel Instance
        {
            get
            {
                if(_Instance == null)
                    _Instance = new MainWindowViewModel();

                return _Instance;
            }
        }

        /// <summary>
        /// Base constructor
        /// </summary>
        private MainWindowViewModel()
        {
            State = MainWindowState.loginPage;
        }

    }
}
