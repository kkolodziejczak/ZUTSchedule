using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public class WeekViewModel : BaseViewModel
    {

        private List<DayViewModel> _days;

        private ObservableCollection<List<DayViewModel>> _weeks;

        /// <summary>
        /// Contains all days of the week
        /// </summary>
        public List<DayViewModel> Days
        {
            get
            {
                return _weeks[Storage.DayShift% _weeks.Count];
            }
            set
            {
                _days = value;
                _weeks.Add(value);
            }
        }

        /// <summary>
        /// Contains news feed
        /// </summary>
        public NewsContainerViewModel News { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public WeekViewModel()
        {
            _weeks = new ObservableCollection<List<DayViewModel>>();
            _days = new List<DayViewModel>();

            RefreshSchedule();
            Storage.Instance.OnDayShiftUpdate += RefreshSchedule;

            var list = new List<INewsService>()
            {
                new WINewsService(),
                new ZUTNewsService(),
            };

            var t = Task.Run(() =>
            {
                News = new NewsContainerViewModel(list);
            });
            t.Wait();
        }

        /// <summary>
        /// Refreshes schedule
        /// </summary>
        public void RefreshSchedule()
        {
            if(_weeks.Count > 0)
            {
                OnPropertyChanged(nameof(Days));

                if (_weeks.First().Count == Storage.NumberOfDaysInTheWeek)
                {
                    return;
                }
            }

            switch (Storage.NumberOfDaysInTheWeek)
            {
                // Logic for 1 day Week
                case 1:

                    var LastDayOfTheYear = Storage.Classes.Last().date;
                    var LastDate = Storage.Classes.First().date;

                    while (DateTime.Compare(LastDate, LastDayOfTheYear) < 0)
                    {
                        var Day = Storage.Classes.Where(day => day.date.DayOfYear == LastDate.DayOfYear).ToList();
                        if (!Day.Any())
                        {
                            Days = new List<DayViewModel>()
                            {
                                new DayViewModel()
                                {
                                    date = LastDate
                                }
                            };
                            LastDate = LastDate.AddDays(1);
                            continue;
                        }

                        Days = new List<DayViewModel>(Day);
                        LastDate = LastDate.AddDays(1);
                    }

                    var FirstDayOfTheClasses = _weeks.First().First().date.DayOfYear;
                    var TodaysDayOfTheYear = DateTime.Now.DayOfYear;
                    Storage.DayShift = TodaysDayOfTheYear - FirstDayOfTheClasses;
                    break;

                // Logic for 5 and 7 week Days
                default:

                    if (Storage.Classes == null)
                        return;

                    var EndOfLastWeek = Storage.Classes.First().date;
                    foreach(var _class in Storage.Classes)
                    {
                        var FirstDayOfTheWeek = EndOfLastWeek.StartOfWeek(DayOfWeek.Monday);

                        var WeekDays = Storage.Classes.Where(day => day.date.DayOfYear >= FirstDayOfTheWeek.DayOfYear
                                                                 && day.date.DayOfYear < FirstDayOfTheWeek.DayOfYear + 7)
                                                      .ToList();

                        var week = GetMissingsDays(WeekDays, FirstDayOfTheWeek);
                        Days = week;

                        EndOfLastWeek = week.Last().date.AddDays(7);
                    }

                    var FirstWeekOfTheYear = _weeks.First().First().date.ToIso8601Weeknumber();
                    var TodaysWeekOfTheYear = DateTime.Now.ToIso8601Weeknumber();
                    Storage.DayShift = TodaysWeekOfTheYear - FirstWeekOfTheYear;

                    break;
            }

            OnPropertyChanged(nameof(Days));
        }

        /// <summary>
        /// Fills <paramref name="week"/> missing days
        /// </summary>
        /// <param name="week"></param>
        /// <param name="FirstDayOfTheWeek"></param>
        /// <returns></returns>
        private List<DayViewModel> GetMissingsDays(List<DayViewModel> week, DateTime FirstDayOfTheWeek)
        {
            var output = new List<DayViewModel>();

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
