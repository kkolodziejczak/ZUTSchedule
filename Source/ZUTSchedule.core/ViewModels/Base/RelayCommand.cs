using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZUTSchedule.core
{
    public class RelayCommand : ICommand
    {
        #region Private Fields

        /// <summary>
        /// Action to run
        /// </summary>
        private Action _Action;

        #endregion

        #region Events

        /// <summary>
        /// The evenet thats fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            _Action = action;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// A relay command can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the commands Action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _Action();
        }

        #endregion
    }
}