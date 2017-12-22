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
        /// All news Downloaded form sites
        /// </summary>
        public ObservableCollection<NewsRecordViewModel> News { get; set; }

        /// <summary>
        /// Base Constructor
        /// </summary>
        public NewsContainerViewModel()
        {
            var t = Task.Run(async () => await DownloadNewsAsync());
            t.Wait();
        }
        
        /// <summary>
        /// Download News from services
        /// </summary>
        private async Task DownloadNewsAsync()
        {
            var NewsFactory = IoC.Get<INewsFactory>();
            
            var DownloadedNews = await NewsFactory.GetNewsAsync();

            News = new ObservableCollection<NewsRecordViewModel>(DownloadedNews);
        }

    }
}
