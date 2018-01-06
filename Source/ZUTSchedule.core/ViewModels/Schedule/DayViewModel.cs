using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

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
        public ObservableCollection<ClassViewModel> Courses { get; set; }

        /// <summary>
        /// Date of the day
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Indicates if DayViewModel contains classes that are today
        /// </summary>
        public bool Today
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
                //                        t1              t2
                return DateTime.Compare(Date.OnlyDate(), DateTimeToCheck.OnlyDate()) == 0;
            }
        }

        /// <summary>
        /// Returns Day of the week in polish Translation
        /// </summary>
        public string DayOfTheWeek
        {
            get
            {
                return Date.GetDayOfWeekPolish();
            }
        }

        public ICommand IncrementCommand { get; private set; }
        public ICommand DecrementCommand { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DayViewModel()
        {
            Courses = new ObservableCollection<ClassViewModel>();

            IncrementCommand = new RelayCommand(IoC.Settings.IncrementWeek);
            DecrementCommand = new RelayCommand(IoC.Settings.DecrementWeek);
        }

    }
}
