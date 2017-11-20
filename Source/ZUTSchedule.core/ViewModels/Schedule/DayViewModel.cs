using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZUTSchedule.core
{
    /// <summary>
    /// ViewModel that represents full day of classes
    /// </summary>
    public class DayViewModel : BaseViewModel
    {
        /// <summary>
        /// Contains all classes of the day
        /// </summary>
        public ObservableCollection<CourseViewModel> Courses { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DayViewModel()
        {

        }
    }
}
