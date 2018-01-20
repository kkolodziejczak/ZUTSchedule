using System.ComponentModel;

namespace ZUTSchedule.core
{
    /// <summary>
    /// Base class for all ViewModels
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Business logic of the application
        /// </summary>
        protected BusinessLogic businessLogic;

        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public BaseViewModel()
        {
            businessLogic = new BusinessLogic();
        }

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}