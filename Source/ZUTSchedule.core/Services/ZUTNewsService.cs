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
        public async Task<IEnumerable<NewsRecordViewModel>> GetNews()
        {
            var output = new List<NewsRecordViewModel>();

            var website = await GetWebsiteContent(@"http://www.zut.edu.pl/zut-studenci/start/aktualnosci.html#");

            var News = Regex.Matches(website,
                "(?:href=\")(.*?)(?:\".*?em>)(.*?)(?:</em>.*?-->)(.*?)(?:<!--)");

            Console.WriteLine(News.Count);

            for (int i = 0; i < News.Count; i++)
            {
                var d = News.getValueAt(i, 2).Trim().Split('.');
                var date = new DateTime(d[2].ToInt(), d[1].ToInt(), d[0].ToInt());

                output.Add(new NewsRecordViewModel()
                {
                    Title = News.getValueAt(i, 3).Trim(),
                    Url = News.getValueAt(i, 1).Trim().Contains("http")? News.getValueAt(i, 1).Trim() : $"http://www.zut.edu.pl{News.getValueAt(i, 1).Trim()}",
                    Type = NewsType.Global,
                    IsNew = date.IsNotOlderThan(3),
                });
            }

            return output;
        }

        private async Task<string> GetWebsiteContent(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var getResponse = await client.GetAsync(url);
                return await getResponse.Content.ReadAsStringAsync();
            }
        }
    }
}
