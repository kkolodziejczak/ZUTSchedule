using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZUTSchedule.core
{
    public class WeekViewModel : BaseViewModel
    {
        /// <summary>
        /// Contains all days of the week
        /// </summary>
        public ObservableCollection<DayViewModel> Days { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public WeekViewModel()
        {
            RefreshSchedule();
            Storage.Instance.OnDayShiftUpdate += RefreshSchedule;
        }

        /// <summary>
        /// Refreshes schedule
        /// </summary>
        public void RefreshSchedule()
        {
            switch (Storage.NumberOfDaysInTheWeek)
            {
                // Logic for 1 day Week
                case 1:
                    var ShiftedDate = DateTime.Now.AddDays(Storage.NumberOfDaysInTheWeek * Storage.DayShift);

                    var Day = Storage.Classes.Where(day => day.date.DayOfYear == ShiftedDate.DayOfYear)
                                                                                 .ToList();
                    if (!Day.Any())
                    {
                        Days = new ObservableCollection<DayViewModel>()
                        {
                            new DayViewModel() { date = ShiftedDate }
                        };
                        return;
                    }

                    Days = new ObservableCollection<DayViewModel>(Day);
                    break;

                // Logic for 5 and 7 week Days
                default:
                    var ShiftedDateTime = DateTime.Now.AddDays(Storage.NumberOfDaysInTheWeek * Storage.DayShift);
                    var FirstDayOfTheWeek = ShiftedDateTime.StartOfWeek(DayOfWeek.Monday);

                    if (Storage.Classes == null)
                        return;

                    var WeekDays = Storage.Classes.Where(day => day.date.DayOfYear >= FirstDayOfTheWeek.DayOfYear
                                                             && day.date.DayOfYear < FirstDayOfTheWeek.DayOfYear + 7)
                                                  .ToList();

                    Days = GetMissingsDays(WeekDays, FirstDayOfTheWeek);
                    break;
            }
        }

        private ObservableCollection<DayViewModel> GetMissingsDays(List<DayViewModel> week, DateTime FirstDayOfTheWeek)
        {
            var output = new ObservableCollection<DayViewModel>();

            //TODO get number of days from Settings! 
            // Also check for 1 day view
            for (int i = 0; i < Storage.NumberOfDaysInTheWeek; i++)
            {
                var day = week.Where(d => d.date == FirstDayOfTheWeek.AddDays(i));
                if (day.Any())
                {
                    output.Add(day.First());
                    continue;
                }

                output.Add(new DayViewModel()
                {
                    date = FirstDayOfTheWeek.AddDays(i),
                });

            }

            return output;
        }

    }

}
