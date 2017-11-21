using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Collections.ObjectModel;
using System.Net;
using System.IO;
using System.Collections.Specialized;

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

            return new List<DayViewModel>()
            {
                new DayViewModel()
                {
                    Courses = new ObservableCollection<CourseViewModel>()
                    {
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                    }
                },
                new DayViewModel()
                {
                    Courses = new ObservableCollection<CourseViewModel>()
                    {
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                    }
                },
                new DayViewModel()
                {
                    Courses = new ObservableCollection<CourseViewModel>()
                    {
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                        new CourseViewModel()
                        {
                            StartTime = "10:15",
                            EndTime = "12:00",
                            CourseName = "Programowanie",
                            ClassroomID = "WI1, 233",
                            TeacherName = "Teacher 1",
                            ClassType = "L"
                        },
                    }
                },
            };
        }

        private async static void PostRequest(string url)
        {

            string login = "Test123";
            string Password = "Password";
            string Typ = "student";

            string posty = $"ctl00_ctl00_ScriptManager1_HiddenField=&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUKMTc3NTQ1OTc2NA8WAh4DaGFzZRYCZg9kFgJmD2QWAgIBD2QWBAIDD2QWAgIBD2QWAgIBD2QWAgICDxQrAAIUKwACDxYEHgtfIURhdGFCb3VuZGceF0VuYWJsZUFqYXhTa2luUmVuZGVyaW5naGRkZGQCBA9kFgICAQ9kFhICAQ8WAh4JaW5uZXJodG1sBSZlLUR6aWVrYW5hdDwhLS0gc3RhdHVzOiA1NjE0NTAxMjYgLS0%2BIGQCDQ8PFgIeBE1vZGULKiVTeXN0ZW0uV2ViLlVJLldlYkNvbnRyb2xzLlRleHRCb3hNb2RlAmRkAhUPDxYEHgRUZXh0BRlPZHp5c2tpd2FuaWUgaGFzxYJhPGJyIC8%2BHgdWaXNpYmxlaGRkAhcPZBYCAgMPEGQPFgJmAgEWAgURc3R1ZGVudC9kb2t0b3JhbnQFCGR5ZGFrdHlrFgFmZAIZD2QWBAIBDw8WAh8FBTQ8YnIgLz5MdWIgemFsb2d1aiBzacSZIGpha28gc3R1ZGVudCBwcnpleiBPZmZpY2UzNjU6ZGQCAw8PFgIfBQUIUHJ6ZWpkxbpkZAIbDw8WBB8FBRhTZXJ3aXMgQWJzb2x3ZW50w7N3PGJyLz4fBmhkZAIfDw8WAh8GaGRkAiEPDxYCHwZoZBYGAgEPDxYCHwVkZGQCAw8PFgIfBQUGQW51bHVqZGQCBQ8PFgIfBQUHUG9iaWVyemRkAiMPDxYCHwUFI1fFgsSFY3ogcmVrbGFtxJkgYXBsaWthY2ppIG1vYmlsbmVqZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFSmN0bDAwJGN0bDAwJFRvcE1lbnVQbGFjZUhvbGRlciRUb3BNZW51Q29udGVudFBsYWNlSG9sZGVyJE1lbnVUb3AzJG1lbnVUb3Az38BzqfstGliCA7rUUjKrKnlhZpc%3D&__VIEWSTATEGENERATOR=7D6A02AE&ctl00_ctl00_TopMenuPlaceHolder_TopMenuContentPlaceHolder_MenuTop3_menuTop3_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtIdent={login}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtHaslo={Password}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24rbKto=student";

            posty += "&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24butLoguj=Zaloguj";

            var message = new HttpRequestMessage(HttpMethod.Post, url);
                message.Headers.Add("Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                message.Headers.Add("Connection", "keep-alive");
                message.Content = new StringContent(posty, Encoding.UTF8, "application/x-www-form-urlencoded");

            using (HttpClient client = new HttpClient())
            {
                    var result = await client.SendAsync(message);
                    var ss2 = await result.Content.ReadAsStringAsync();
            }
        }
    }
}
