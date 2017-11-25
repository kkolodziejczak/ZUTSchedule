using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Collections.ObjectModel;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ZUTSchedule.core
{
    /// <summary>
    /// Data service to get data from e-dziekanat
    /// </summary>
    public class EDziekanatService
    {
        
        /// <summary>
        /// Returns list of days with classes
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public async Task<List<DayViewModel>> getClasses(List<DateTime> dates)
        {

            string SiteDocumentWithSchedule = string.Empty;

            // Login and get site html with schedule
            using (HttpClientHandler handle = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer(),
            })
            using (HttpClient client = new HttpClient(handle))
            {

                var LoginRequestContent = new StringContent(Storage.POST_LoginMessage, Encoding.UTF8, "application/x-www-form-urlencoded");

                string SiteDocument = await PostRequest(Storage.LoginURL, LoginRequestContent, client);

                if (LoggedIn(SiteDocument) == false)
                {
                    // User is not logged in something went wrong
                    //TODO: Signalize?
                    return new List<DayViewModel>();
                }

                var SwitchModeRequestContent = new StringContent(Storage.POST_SwitchModeMessage, Encoding.UTF8, "application/x-www-form-urlencoded");

                SiteDocumentWithSchedule = await PostRequest(Storage.PlanURL, SwitchModeRequestContent, client);

                // Log out of the system
                var LogOut = await client.GetAsync(Storage.LogOutURL);

            }

            return GenerateDaysFromSiteDocument(SiteDocumentWithSchedule);
        }

        /// <summary>
        /// Send POST request to <paramref name="url"/> with <paramref name="content"/> using <paramref name="client"/>
        /// </summary>
        /// <param name="url">url to send content to</param>
        /// <param name="content">content to send</param>
        /// <param name="client">client reference to use</param>
        /// <returns></returns>
        private async Task<string> PostRequest(string url, HttpContent content, HttpClient client)
        {
            string Response = string.Empty;

            using (HttpResponseMessage HTTPResponse = await client.PostAsync(url, content))
            {
                using (HttpContent ReturnedContent = HTTPResponse.Content)
                {
                    Response = await ReturnedContent.ReadAsStringAsync();
                }
            }

            return Response;
        }

        /// <summary>
        /// Checks if user is logged in
        /// </summary>
        /// <param name="siteHTML"></param>
        /// <returns></returns>
        private bool LoggedIn(string siteHTML)
        {
            var logged = Regex.Matches(siteHTML, "WhoIsLoggedIn");
            return logged.Count != 0;
        }

        /// <summary>
        /// Returns list of <see cref="DayViewModel"/> from Schedule html
        /// </summary>
        /// <param name="siteHTML"></param>
        /// <returns></returns>
        private List<DayViewModel> GenerateDaysFromSiteDocument(string siteHTML)
        {
            if (string.IsNullOrWhiteSpace(siteHTML))
            {
                return null;
            }

            var Result = new List<DayViewModel>();

            // Get html table with all classes
            var TableWithClasses = Regex.Matches(siteHTML, 
                "<table.*?\\\"ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_dgDane\\\".*?><tbody>.*?</tbody></table>");

            if(TableWithClasses.Count == 0)
            {
                return null;
            }

            // Get record form table
            var Records = Regex.Matches(TableWithClasses[0].Value, 
                "<tr class=\\\"gridDane\\\">(?:<td(?:.*?)>?(?:(?:<a.*?>)*(.*?)(?:</a>)*)</td>?)</tr>");

            // Generate all classes from records
            foreach(var record in Records)
            {
                var RecordData = Regex.Matches(record.ToString(),
                    "(?:<td.*?>(?:(?:<font.*?>)*(?:<a.*?>)*(.*?)(?:</a>)*(?:</font>)*)</td>?)");

                var ClassDateTime = Regex.Matches(RecordData.getValueAt(0), "(\\d+)\\.(\\d+)\\.(\\d+).*").ToDateTime();

                // If there is no day with this date add new day
                if (Result.Where(date => date.date == ClassDateTime).Count() == 0)
                {
                    Result.Add(new DayViewModel() { date = ClassDateTime });
                }

                // add class to last added day
                Result.Last().Courses.Add(new CourseViewModel()
                {
                    StartTime = RecordData.getValueAt(ClassRecordData.StartTime),
                    EndTime = RecordData.getValueAt(ClassRecordData.EndTime),
                    ClassroomID = RecordData.getValueAt(ClassRecordData.Place),
                    TeacherName = RecordData.getValueAt(ClassRecordData.Teacher),
                    CourseName = $"{RecordData.getValueAt(ClassRecordData.Course)} ({RecordData.getValueAt(ClassRecordData.CourseType)})",
                    Status = RecordData.getValueAt(ClassRecordData.Status),
                    Date = ClassDateTime,
                });

            }

            return Result;
        }

    }
}
