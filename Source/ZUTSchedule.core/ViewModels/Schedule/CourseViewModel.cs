using System;
using System.Collections.Generic;
using System.Text;

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
        /// Course Type 
        /// i.e. Laboratory, Lecture, ...
        /// </summary>
        public string ClassType { get; set; }

        /// <summary>
        /// Teachers name
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// ID of the classroom where course will be
        /// </summary>
        public string ClassroomID { get; set; }

    }
}
