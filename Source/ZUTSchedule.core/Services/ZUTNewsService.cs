using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public class ZUTNewsService : INewsService
    {
        /// <summary>
        /// Returns news from ZUT site
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NewsRecordViewModel>> GetNews()
        {
            var output = new List<NewsRecordViewModel>();
            var settings = IoC.Settings;

            var website = await GetWebsiteContent(settings.newsZutURL);

            var news = Regex.Matches(website,
                "(?:href=\")(.*?)(?:\".*?em>)(.*?)(?:</em>.*?-->)(.*?)(?:<!--)");

            for (int i = 0; i < news.Count; i++)
            {
                var d = news.getValueAt(i, 2).Trim().Split('.');
                var date = new DateTime(d[2].ToInt(), d[1].ToInt(), d[0].ToInt());

                var title = news.getValueAt(i, 3).Trim();
                if(title.Length > settings.HowLongNewsMessages)
                {
                    title = title.Substring(0, settings.HowLongNewsMessages);
                }

                output.Add(new NewsRecordViewModel()
                {
                    Title = $"{title}...",
                    Href = news.getValueAt(i, 1).Trim().Contains("http")? news.getValueAt(i, 1).Trim() : $"http://www.zut.edu.pl{news.getValueAt(i, 1).Trim()}",
                    Type = NewsType.Global,
                    IsNew = date.IsNotOlderThan(settings.HowManyDaysIsNew),
                });
            }

            return output;
        }

        /// <summary>
        /// Returns content of the website provided by <paramref name="url"/>
        /// </summary>
        /// <param name="url">URL to website that you want to get content of.</param>
        /// <returns></returns>
        private async Task<string> GetWebsiteContent(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage getResponse = await client.GetAsync(url))
                {
                    return await getResponse.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
