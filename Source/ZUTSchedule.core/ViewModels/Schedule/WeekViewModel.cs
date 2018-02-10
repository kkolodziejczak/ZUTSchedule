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

        private Storage _settings;
        private ObservableCollection<List<DayViewModel>> _weeks;

        /// <summary>
        /// Contains all days of the week
        /// </summary>
        public List<DayViewModel> Days
        {
            get
            {
                return _weeks[_settings.DayShift% _weeks.Count];
            }
            set
            {
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
            _settings = IoC.Settings;
            News = new NewsContainerViewModel();

            // Download 
            if(_settings.Classes == null || _settings.Classes.Count == 0)
            {
                Task.Run(GetClasses).Wait();
            }

            _settings.OnDayShiftUpdate += RefreshSchedule;
            _settings.OnRefresh += ReloadSchedule;
            RefreshSchedule();

        }

        public async void ReloadSchedule()
        {
            await GetClasses();
            RefreshSchedule();
        }

        /// <summary>
        /// Get Download classes
        /// </summary>
        /// <returns></returns>
        private async Task GetClasses()
        {
            _settings.Classes = await businessLogic.GetClassesAsync();
        }

        /// <summary>
        /// Refreshes schedule
        /// </summary>
        public void RefreshSchedule()
        {
            if(_weeks.Count > 0)
            {
                OnPropertyChanged(nameof(Days));

                if (_weeks.First().Count == _settings.NumberOfDaysInTheWeek)
                {
                    return;
                }
            }

            switch (_settings.NumberOfDaysInTheWeek)
            {
                // Logic for 1 day Week
                case 1:

                    var lastDayOfTheYear = _settings.Classes.Last().Date;
                    var lastDate = _settings.Classes.First().Date;

                    while (DateTime.Compare(lastDate, lastDayOfTheYear) < 0)
                    {
                        var day = _settings.Classes.Where(d => d.Date.DayOfYear == lastDate.DayOfYear).ToList();
                        if (!day.Any())
                        {
                            Days = new List<DayViewModel>()
                            {
                                new DayViewModel()
                                {
                                    Date = lastDate
                                }
                            };
                            lastDate = lastDate.AddDays(1);
                            continue;
                        }

                        Days = new List<DayViewModel>(day);
                        lastDate = lastDate.AddDays(1);
                    }

                    var firstDate = _weeks.First().First().Date;
                    var firstDayOfTheClasses = firstDate.DayOfYear;
                    var todaysDayOfTheYear = DateTime.Now.DayOfYear + (firstDate.Year < DateTime.Now.Year ? DateTime.Now.DaysInTheYear(firstDate.Year) : 0);
                    _settings.DayShift = todaysDayOfTheYear - firstDayOfTheClasses;
                    break;

                // Logic for 5 and 7 week Days
                default:

                    if (_settings.Classes == null)
                        return;

                    var endOfLastWeek = _settings.Classes.First().Date;
                    foreach(var _class in _settings.Classes)
                    {
                        var firstDayOfTheWeek = endOfLastWeek.StartOfWeek(DayOfWeek.Monday);

                        var weekDays = _settings.Classes.Where(day => day.Date.DayOfYear >= firstDayOfTheWeek.DayOfYear
                                                                 && day.Date.DayOfYear < firstDayOfTheWeek.DayOfYear + 7)
                                                      .ToList();

                        var week = GetMissingsDays(weekDays, firstDayOfTheWeek);
                        Days = week;

                        endOfLastWeek = week.Last().Date.AddDays(7);
                    }

                    var firstWeeksDate = _weeks.First().First().Date;
                    var firstWeekOfTheYear = firstWeeksDate.ToIso8601Weeknumber();
                    // In case of new year add 52 weeks else 0
                    var todaysWeekOfTheYear = DateTime.Now.ToIso8601Weeknumber() + (DateTime.Now.Year > firstWeeksDate.Year ? 52 : 0);
                    _settings.DayShift = todaysWeekOfTheYear - firstWeekOfTheYear;

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

            for (int i = 0; i < _settings.NumberOfDaysInTheWeek; i++)
            {
                var day = week.Where(d => d.Date == FirstDayOfTheWeek.AddDays(i));
                if (day.Any())
                {
                    output.Add(day.First());
                    continue;
                }

                output.Add(new DayViewModel()
                {
                    Date = FirstDayOfTheWeek.AddDays(i),
                });

            }

            return output;
        }

    }

}
