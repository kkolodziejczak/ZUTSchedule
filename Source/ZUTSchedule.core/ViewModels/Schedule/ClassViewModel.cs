using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ZUTSchedule.core
{
    /// <summary>
    /// ViewModel that represents single course
    /// </summary>
    public class ClassViewModel : BaseViewModel
    {

        /// <summary>
        /// Starting time of the course
        /// </summary>
        public DateTime StartTime { get; set; }

        public string StartTimeString
        {
            get => StartTime.ToString("HH:mm");
        }

        /// <summary>
        /// Ending time of the course
        /// </summary>
        public DateTime EndTime { get; set; }

        public string EndTimeString
        {
            get => EndTime.ToString("HH:mm");
        }

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

        /// <summary>
        /// Indicates if class is right now
        /// </summary>
        public bool Now
        {
            get
            {

#if DEBUG
                DateTime DateTimeToCheck = new DateTime(DateTime.Now.StartOfWeek().Year,
                                                        DateTime.Now.StartOfWeek().Month,
                                                        DateTime.Now.StartOfWeek().Day,
                                                        10,
                                                        0,
                                                        0);
#else
                DateTime DateTimeToCheck = DateTime.Now;
#endif
                // <  0     t1 is earlier than t2
                // == 0     t1 is the same as t2
                // >  0     t1 i later than t2
                //                                      t1              t2
                var isOlderThanStart = DateTime.Compare(DateTimeToCheck, StartTime) >= 0;

                //                                      t1              t2
                var isEalierThanEnd = DateTime.Compare(DateTimeToCheck, EndTime) <= 0;

                return isOlderThanStart && isEalierThanEnd;
            }
        }
    }
}
