using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public class WINewsService : INewsService
    {

        public async Task<IEnumerable<NewsRecordViewModel>> GetNews()
        {
            var output = new List<NewsRecordViewModel>();

            var website = await GetWebsiteContent(@"https://www.wi.zut.edu.pl/index.php/pl/dla-studenta/sprawy-studenckie/aktualnosci-studenckie");

            var News = Regex.Matches(website,
                "(?:<h2 itemprop=\"name\">\\s*?<a href=\")(.*?)(?:\".*?>\\s*?)(.*?)(?:<\\/a>)");

            var Dates = Regex.Matches(website,"datetime=\"(.*?)T");

            for(int i = 0; i < News.Count; i++)
            {
                var d = Dates.getValueAt(i, 1).Trim().Split('-');
                var date = new DateTime(d[0].ToInt(), d[1].ToInt(), d[2].ToInt());

                output.Add(new NewsRecordViewModel()
                {
                    Title = News.getValueAt(i, 2).Trim(),
                    Url = $"https://www.wi.zut.edu.pl{News.getValueAt(i, 1).Trim()}",
                    Type = NewsType.Wi,
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
