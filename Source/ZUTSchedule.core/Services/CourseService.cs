using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ZUTSchedule.core
{

    public class CourseService
    {
        
        /// <summary>
        /// Returns list of days with courses
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public List<DayViewModel>  getCourses(List<DateTime> dates)
        {
            //
            //PostRequest("https://posttestserver.com/post.php");
            PostRequest("https://www.zut.edu.pl/WU/Logowanie2.aspx");
            
            return new List<DayViewModel>();
        }

        private async static void GetRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using(HttpResponseMessage response = await client.GetAsync(url))
                {
                    var s = response.Content;
                }
                
            }

        }

        private async static void PostRequest(string url)
        {

            string login = "Login";
            string Password = "Password";
            string Typ = "student";
            //string Typ = "dydaktyk";

            string strings = $"&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtIdent={login}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtHaslo={Password}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24butLoguj=Zaloguj&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24rbKto={Typ}";
            var s = System.Text.Encoding.UTF8.GetBytes(strings);


            HttpContent cont = new ByteArrayContent(s);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(url, cont))
                {
                    var req = response.RequestMessage;
                    using(HttpContent content = response.Content)
                    {
                        string ss = await content.ReadAsStringAsync();
                        var headers = content.Headers;
                    }
                }
            }
        }
    }
}
