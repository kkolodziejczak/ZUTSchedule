using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public class NewsFactory : INewsFactory
    {
        private List<INewsService> _services = new List<INewsService>();

        /// <summary>
        /// Adds new news service
        /// </summary>
        /// <param name="service">Service to add</param>
        public void AddNewsService(INewsService service)
        {
            _services.Add(service);
        }
        
        /// <summary>
        /// Removes news service
        /// </summary>
        /// <param name="service">Service to remove</param>
        public void RemoveNewsService(INewsService service)
        {
            if(_services != null)
            {
                _services.Remove(service);
            }
        }

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="services"></param>
        public NewsFactory(INewsService [] services = null)
        {
            if (services != null)
            {
                foreach (var service in services)
                {
                    AddNewsService(service);
                }
            }
        }

        /// <summary>
        /// Returns <see cref="IEnumerable{NewsRecordViewModel}"/>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NewsRecordViewModel>> GetNewsAsync()
        {
            var list = new List<NewsRecordViewModel>();
            foreach (var service in _services)
            {
                var DownloadedNews = await service.GetNews();
                foreach (var news in DownloadedNews)
                {
                    list.Add(news);
                }
            }

            return list;
        }

    }
}
