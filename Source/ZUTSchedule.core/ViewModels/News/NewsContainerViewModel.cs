using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public class NewsContainerViewModel :BaseViewModel
    {
        /// <summary>
        /// List of services to get news from
        /// </summary>
        private List<INewsService> _Services;

        /// <summary>
        /// All news Downloaded form sites
        /// </summary>
        public ObservableCollection<NewsRecordViewModel> News { get; set; }


        /// <summary>
        /// Base Constructor
        /// </summary>
        public NewsContainerViewModel(List<INewsService> services)
        {
            //TODO: normalize Titles
            _Services = services;

            var t = Task.Run(async () => await DownloadNews());
            t.Wait();
        }
        
        /// <summary>
        /// Download News from services
        /// </summary>
        private async Task DownloadNews()
        {
            var list = new List<NewsRecordViewModel>();
            foreach (var service in _Services)
            {
                var DownloadedNews = await service.GetNews();
                foreach (var news in DownloadedNews)
                {
                    list.Add(news);
                }
            }

            News = new ObservableCollection<NewsRecordViewModel>(list);
        }

    }
}
