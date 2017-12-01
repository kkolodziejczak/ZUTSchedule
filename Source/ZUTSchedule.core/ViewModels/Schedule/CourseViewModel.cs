using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ZUTSchedule.core
{
    /// <summary>
    /// ViewModel that represents single course
    /// </summary>
    public class CourseViewModel : BaseViewModel
    {

        /// <summary>
        /// Starting time of the course
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Ending time of the course
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Course name
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Teachers name
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// ID of the classroom where course will be
        /// </summary>
        public string ClassroomID { get; set; }

        /// <summary>
        /// Status of the class
        ///     1. Cancelled
        ///     2. Exam
        ///     3. Rector hours
        /// </summary>
        public string Status { get; set; }

        private DateTime _Date;

        /// <summary>
        /// Date of the class
        /// </summary>
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date == value)
                    return;

                _Date = value;

                Now = _Date.IsRightNow();
            }
        }

        public bool Now { get; set; }
    }
}
