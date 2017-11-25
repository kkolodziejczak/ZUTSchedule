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


        public WeekViewModel()
        {
            int howManyDays = Storage.NumberOfDaysInTheWeek;
            var HowManyWeekShift = 0;

            var ShiftedDateTime = DateTime.Now.AddDays(howManyDays * HowManyWeekShift);
            var FirstDayOfTheWeek = ShiftedDateTime.StartOfWeek(DayOfWeek.Monday);

            if (Storage.Classes == null)
                return;

            var WeekDays = Storage.Classes.Where(day => day.date.DayOfYear >= FirstDayOfTheWeek.DayOfYear
                                                     && day.date.DayOfYear < FirstDayOfTheWeek.DayOfYear + 7)
                                          .ToList();

            Days = GetMissingsDays(WeekDays);

        }

        private ObservableCollection<DayViewModel> GetMissingsDays(List<DayViewModel> week)
        {
            var output = new ObservableCollection<DayViewModel>();

            var FirstDayOfTheWeek = week.First().date.StartOfWeek(DayOfWeek.Monday);

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
