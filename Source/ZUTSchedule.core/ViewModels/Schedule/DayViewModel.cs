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
        public ObservableCollection<CourseViewModel> Courses { get; set; }

        /// <summary>
        /// Date of the day
        /// </summary>
        public DateTime date { get; set; }

        public string DayOfTheWeek
        {
            get { return date.DayOfWeek.ToString(); }
        }

        public DayViewModel()
        {
            Courses = new ObservableCollection<CourseViewModel>();
        }

    }
}
