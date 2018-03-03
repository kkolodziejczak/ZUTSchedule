using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    public class BusinessLogic
    {
        private CommunicationService _communicationService;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BusinessLogic()
        {
            _communicationService = new CommunicationService();
        }

        /// <summary>
        /// Attempt to login user to e-Dziekanat system
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        /// <exception cref="CredentialException"/>
        public async Task<string> LoginAsync(Credential creditials)
        {
            try
            {
                var result = await _communicationService.SendAsync(IoC.Settings.loginURL, GenerateLoginContent(creditials));
                CheckIfLoggedIn(result);
                return result;
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"Login attempt failed \n {ex.Message} \n\n {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Attempt to logout user from e-Dziekanat system
        /// </summary>
        /// <returns></returns>
        public async Task<string> LogoutAsync()
        {
            try
            {
                var siteAfterLogOut = await _communicationService.SendAsync(IoC.Settings.logOutURL);
                CheckIfLoggedIn(siteAfterLogOut);
                return siteAfterLogOut;
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"Logout attempt failed \n {ex.Message} \n\n {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Generates POST content for login
        /// </summary>
        /// <param name="userCredential"></param>
        /// <returns></returns>
        private HttpContent GenerateLoginContent(Credential userCredential)
        {
            return new StringContent($"ctl00_ctl00_ScriptManager1_HiddenField=&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUKMTc3NTQ1OTc2NA8WAh4DaGFzZRYCZg9kFgJmD2QWAgIBD2QWBAIDD2QWAgIBD2QWAgIBD2QWAgICDxQrAAIUKwACDxYEHgtfIURhdGFCb3VuZGceF0VuYWJsZUFqYXhTa2luUmVuZGVyaW5naGRkZGQCBA9kFgICAQ9kFhICAQ8WAh4JaW5uZXJodG1sBSZlLUR6aWVrYW5hdDwhLS0gc3RhdHVzOiA1NjE0NTAxMjYgLS0%2BIGQCDQ8PFgIeBE1vZGULKiVTeXN0ZW0uV2ViLlVJLldlYkNvbnRyb2xzLlRleHRCb3hNb2RlAmRkAhUPDxYEHgRUZXh0BRlPZHp5c2tpd2FuaWUgaGFzxYJhPGJyIC8%2BHgdWaXNpYmxlaGRkAhcPZBYCAgMPEGQPFgJmAgEWAgURc3R1ZGVudC9kb2t0b3JhbnQFCGR5ZGFrdHlrFgFmZAIZD2QWBAIBDw8WAh8FBTQ8YnIgLz5MdWIgemFsb2d1aiBzacSZIGpha28gc3R1ZGVudCBwcnpleiBPZmZpY2UzNjU6ZGQCAw8PFgIfBQUIUHJ6ZWpkxbpkZAIbDw8WBB8FBRhTZXJ3aXMgQWJzb2x3ZW50w7N3PGJyLz4fBmhkZAIfDw8WAh8GaGRkAiEPDxYCHwZoZBYGAgEPDxYCHwVkZGQCAw8PFgIfBQUGQW51bHVqZGQCBQ8PFgIfBQUHUG9iaWVyemRkAiMPDxYCHwUFI1fFgsSFY3ogcmVrbGFtxJkgYXBsaWthY2ppIG1vYmlsbmVqZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFSmN0bDAwJGN0bDAwJFRvcE1lbnVQbGFjZUhvbGRlciRUb3BNZW51Q29udGVudFBsYWNlSG9sZGVyJE1lbnVUb3AzJG1lbnVUb3Az38BzqfstGliCA7rUUjKrKnlhZpc%3D&__VIEWSTATEGENERATOR=7D6A02AE&ctl00_ctl00_TopMenuPlaceHolder_TopMenuContentPlaceHolder_MenuTop3_menuTop3_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtIdent={userCredential.UserName}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtHaslo={userCredential.Password.Unsecure()}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24rbKto={IoC.Settings.Type}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24butLoguj=Zaloguj",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Checks if user is logged in
        /// </summary>
        /// <param name="siteHTML"></param>
        /// <returns></returns>
        private bool CheckIfLoggedIn(string siteHTML)
        {
            if (string.IsNullOrWhiteSpace(siteHTML))
            {
                return false;
            }

            var badUsernameOrPassword = Regex.Matches(siteHTML, "Zła nazwa użytkownika lub hasło");
            if (badUsernameOrPassword.Count != 0)
            {
                throw new CredentialException();
            }

            var logged = Regex.Matches(siteHTML, "WhoIsLoggedIn");
            IoC.Settings.IsUserLoggedIn = logged.Count != 0;
            return IoC.Settings.IsUserLoggedIn;
        }

        /// <summary>
        /// Returns list of all classes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        public async Task<List<DayViewModel>> GetClassesAsync()
        {
            string siteDocumentWithSchedule = string.Empty;

            try
            {
                if (IoC.Settings.IsUserLoggedIn == false)
                {
                    siteDocumentWithSchedule = await LoginAsync(IoC.Settings.UserCredential);
                }

                // Check if student have anything to chose from
                var courses = GetListOfAvailableCourses(siteDocumentWithSchedule);
                if (courses.IsNotNullOrEmpty())
                {
                    // If so pick the latest one
                    var latestCourseMessage = new StringContent(GenerateMessageForLatestCourse(courses),
                       Encoding.UTF8,
                       "application/x-www-form-urlencoded");
                    siteDocumentWithSchedule = await _communicationService.SendAsync(IoC.Settings.LevelURL, latestCourseMessage);
                }

                // Go to schedule in semester view
                var semesterViewMessage = new StringContent(GenerateScheduleLinkForSemester(),
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");
                siteDocumentWithSchedule = await _communicationService.SendAsync(IoC.Settings.scheduleURL, semesterViewMessage);

                // Log out
                var siteAfterLogout = await LogoutAsync();
            }
            catch (HttpRequestException ex)
            {
                Logger.Error("Error while switching to schedule page");
                throw;
            }

            return GenerateDaysFromSiteDocument(siteDocumentWithSchedule);
        }

        /// <summary>
        /// Generates PostMessage to Enter latest course
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        private string GenerateMessageForLatestCourse(IEnumerable<int> courses)
        {
            var level = courses.Last();
            return $"ctl00_ctl00_ScriptManager1_HiddenField=&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUKLTU5NTE2OTE0Ng8WAh4WUHJ6eW5hbGV6bm9zY05hendhS2llchUCIyAoaW5mb3JtYXR5a2EgLSBzdHVkaWEgc3RhY2pvbmFybmUpIyAoaW5mb3JtYXR5a2EgLSBzdHVkaWEgc3RhY2pvbmFybmUpFgJmD2QWAmYPZBYEZg9kFgICBg9kFgICAw8PFgIeB1Zpc2libGVoZGQCAQ9kFgQCAw9kFgRmD2QWAgICDxQrAAIUKwACDxYEHgtfIURhdGFCb3VuZGceF0VuYWJsZUFqYXhTa2luUmVuZGVyaW5naGQPFCsAAxQrAAIPFggeBFRleHQFF1JvemvFgmFkIHphasSZxIcgKHBsYW4pHgtOYXZpZ2F0ZVVybAUTL1dVL1BvZHpHb2R6aW4uYXNweB4FVmFsdWUFF1JvemvFgmFkIHphasSZxIcgKHBsYW4pHgdUb29sVGlwBSxXecWbd2lldGxhIGFrdHVhbG55IHJvemvFgmFkIHphasSZxIcgKHBsYW4pLmRkFCsAAg8WCB8EBQVPY2VueR8FBQ8vV1UvT2NlbnlQLmFzcHgfBgUFT2NlbnkfBwUjV3nFm3dpZXRsYSBsaXN0xJkgb3RyenltYW55Y2ggb2Nlbi5kZBQrAAIPFggfBAUMV3lsb2d1aiBtbmllHwUFEC9XVS9XeWxvZ3VqLmFzcHgfBgUMV3lsb2d1aiBtbmllHwcFIFd5bG9nb3d1amUgcHJhY293bmlrYSB6IHNlcndpc3UuZGQPFCsBA2ZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNGQWBmYPDxYIHwQFF1JvemvFgmFkIHphasSZxIcgKHBsYW4pHwUFEy9XVS9Qb2R6R29kemluLmFzcHgfBgUXUm96a8WCYWQgemFqxJnEhyAocGxhbikfBwUsV3nFm3dpZXRsYSBha3R1YWxueSByb3prxYJhZCB6YWrEmcSHIChwbGFuKS5kZAIBDw8WCB8EBQVPY2VueR8FBQ8vV1UvT2NlbnlQLmFzcHgfBgUFT2NlbnkfBwUjV3nFm3dpZXRsYSBsaXN0xJkgb3RyenltYW55Y2ggb2Nlbi5kZAICDw8WCB8EBQxXeWxvZ3VqIG1uaWUfBQUQL1dVL1d5bG9ndWouYXNweB8GBQxXeWxvZ3VqIG1uaWUfBwUgV3lsb2dvd3VqZSBwcmFjb3duaWthIHogc2Vyd2lzdS5kZAIBD2QWAgICDxQrAAIUKwACDxYEHwJnHwNoZA8UKwABFCsAAg8WCB8EBQdXeWxvZ3VqHwUFEC9XVS9XeWxvZ3VqLmFzcHgfBgUHV3lsb2d1ah8HBRVXeWxvZ293dWplIHogc2Vyd2lzdS5kZA8UKwEBZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNGQWAmYPDxYIHwQFB1d5bG9ndWofBQUQL1dVL1d5bG9ndWouYXNweB8GBQdXeWxvZ3VqHwcFFVd5bG9nb3d1amUgeiBzZXJ3aXN1LmRkAgQPZBYEZg9kFgJmDxQrAAIUKwACDxYIHg1EYXRhVGV4dEZpZWxkBQRUZXh0HhREYXRhTmF2aWdhdGVVcmxGaWVsZAULTmF2aWdhdGVVcmwfAmcfA2hkDxQrAAkUKwACDxYEHwQFB1N0dWRlbnQfBWVkEBYMZgIBAgICAwIEAgUCBgIHAggCCQIKAgsWDBQrAAIPFgQfBAUMRGFuZSBvZ8OzbG5lHwUFDX4vV3luaWsyLmFzcHhkZBQrAAIPFgQfBAUVS29udGFrdCB6IER6aWVrYW5hdGVtHwUFEH4vRHppZWthbmF0LmFzcHhkZBQrAAIPFgQfBAUMVG9rIHN0dWRpw7N3HwUFEX4vVG9rU3R1ZGlvdy5hc3B4ZGQUKwACDxYEHwQFEVByemViaWVnIHN0dWRpw7N3HwUFFn4vUHJ6ZWJpZWdTdHVkaW93LmFzcHhkZBQrAAIPFgQfBAUFT2NlbnkfBQUNfi9PY2VueVAuYXNweGRkFCsAAg8WBB8EBRBPY2VueSBjesSFc3Rrb3dlHwUFEX4vT2NlbnlDemFzdC5hc3B4ZGQUKwACDxYEHwQFC1Byb3dhZHrEhWN5HwUFEX4vUHJvd2FkemFjeS5hc3B4ZGQUKwACDxYEHwQFDVBsYW4gc3R1ZGnDs3cfBQUSfi9QbGFuU3R1ZGlvdy5hc3B4ZGQUKwACDxYEHwQFDFBsYW4gemFqxJnEhx8FBRF%2BL1BvZHpHb2R6aW4uYXNweGRkFCsAAg8WBB8EBQlTdHlwZW5kaWEfBQUQfi9TdHlwZW5kaWEuYXNweGRkFCsAAg8WBB8EBQ9XeWRydWtpL3BvZGFuaWEfBQUOfi9XeWRydWtpLmFzcHhkZBQrAAIPFgQfBAUIUHJha3R5a2kfBQUPfi9QcmFrdHlraS5hc3B4ZGQPFgxmZmZmZmZmZmZmZmYWAQV0VGVsZXJpay5XZWIuVUkuUmFkTWVudUl0ZW0sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwACDxYEHwQFFFR3b2plIGRhbmUgZmluYW5zb3dlHwVlZBAWAWYWARQrAAIPFgQfBAUMTmFsZcW8bm%2FFm2NpHwUFEX4vTmFsZXpub3NjaS5hc3B4ZGQPFgFmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAg8WBB8EBQZEeXBsb20fBWVkEBYDZgIBAgIWAxQrAAIPFgQfBAUaUHJhY2EgZHlwbG9tb3dhIGluZm9ybWFjamUfBQUPfi9QcmFjYUR5cC5hc3B4ZGQUKwACDxYEHwQFC0R5cGxvbSBkYW5lHwUFEX4vRHlwbG9tRGFuZS5hc3B4ZGQUKwACDxYEHwQFDER5cGxvbSBwbGlraR8FBRJ%2BL0R5cGxvbVBsaWtpLmFzcHhkZA8WA2ZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAIPFgQfBAULV2lhZG9tb8WbY2kfBWVkEBYDZgIBAgIWAxQrAAIPFgQfBAUSTWFpbCBkbyBEemlla2FuYXR1HwUFF34vTWFpbERvRHppZWthbmF0dS5hc3B4ZGQUKwACDxYEHwQFDk9nxYJvc3plbmlhIFdVHwUFEX4vT2dsb3N6ZW5pYS5hc3B4ZGQUKwACDxYEHwQFDEFrdHVhbG5vxZtjaR8FBQt%2BL05ld3MuYXNweGRkDxYDZmZmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAg8WBB8EBRJBbmtpZXR5IC8gRWd6YW1pbnkfBWVkEBYCZgIBFgIUKwACDxYEHwQFB0Fua2lldHkfBQUafi9Qb2thekFua0Vnei5hc3B4P3R5cD1hbmtkZBQrAAIPFgQfBAUIRWd6YW1pbnkfBQUafi9Qb2thekFua0Vnei5hc3B4P3R5cD1lZ3pkZA8WAmZmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAg8WBB8EBQZXeWLDs3IfBWVkEBYDZgIBAgIWAxQrAAIPFgQfBAUKV3liw7NyIFctRh8FBRx%2BL1JlemVyd2FjamFQcnplZG1pb3Rvdy5hc3B4ZGQUKwACDxYEHwQFKVd5YsOzciBibG9rw7N3IGkgcHJ6ZWRtaW90w7N3IG9iaWVyYWxueWNoHwUFGX4vV3lib3JQcnplZG1pb3Rvd0NDLmFzcHhkZBQrAAIPFgQfBAUUV3liw7NyIHNwZWNqYWxub8WbY2kfBQUhfi9XeWJvclNwZWNqYWxuU3BlY2phbGl6YWNqaS5hc3B4ZGQPFgNmZmYWAQV0VGVsZXJpay5XZWIuVUkuUmFkTWVudUl0ZW0sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwACDxYEHwQFDFd5c3p1a2l3YXJrYR8FBRN%2BL1d5c3p1a2l3YXJrYS5hc3B4ZGQUKwACDxYEHwQFDlpVVCBFLWxlYXJuaW5nHwUFFX4vTW9vZGxlUmVkaXJlY3QuYXNweGRkFCsAAg8WBB8EBQdXeWxvZ3VqHwUFDn4vV3lsb2d1ai5hc3B4ZGQPFCsBCWZmZmZmZmZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNGQWEmYPDxYEHwQFB1N0dWRlbnQfBWVkFhhmDw8WBB8EBQxEYW5lIG9nw7NsbmUfBQUNfi9XeW5pazIuYXNweGRkAgEPDxYEHwQFFUtvbnRha3QgeiBEemlla2FuYXRlbR8FBRB%2BL0R6aWVrYW5hdC5hc3B4ZGQCAg8PFgQfBAUMVG9rIHN0dWRpw7N3HwUFEX4vVG9rU3R1ZGlvdy5hc3B4ZGQCAw8PFgQfBAURUHJ6ZWJpZWcgc3R1ZGnDs3cfBQUWfi9QcnplYmllZ1N0dWRpb3cuYXNweGRkAgQPDxYEHwQFBU9jZW55HwUFDX4vT2NlbnlQLmFzcHhkZAIFDw8WBB8EBRBPY2VueSBjesSFc3Rrb3dlHwUFEX4vT2NlbnlDemFzdC5hc3B4ZGQCBg8PFgQfBAULUHJvd2FkesSFY3kfBQURfi9Qcm93YWR6YWN5LmFzcHhkZAIHDw8WBB8EBQ1QbGFuIHN0dWRpw7N3HwUFEn4vUGxhblN0dWRpb3cuYXNweGRkAggPDxYEHwQFDFBsYW4gemFqxJnEhx8FBRF%2BL1BvZHpHb2R6aW4uYXNweGRkAgkPDxYEHwQFCVN0eXBlbmRpYR8FBRB%2BL1N0eXBlbmRpYS5hc3B4ZGQCCg8PFgQfBAUPV3lkcnVraS9wb2RhbmlhHwUFDn4vV3lkcnVraS5hc3B4ZGQCCw8PFgQfBAUIUHJha3R5a2kfBQUPfi9QcmFrdHlraS5hc3B4ZGQCAQ8PFgQfBAUUVHdvamUgZGFuZSBmaW5hbnNvd2UfBWVkFgJmDw8WBB8EBQxOYWxlxbxub8WbY2kfBQURfi9OYWxlem5vc2NpLmFzcHhkZAICDw8WBB8EBQZEeXBsb20fBWVkFgZmDw8WBB8EBRpQcmFjYSBkeXBsb21vd2EgaW5mb3JtYWNqZR8FBQ9%2BL1ByYWNhRHlwLmFzcHhkZAIBDw8WBB8EBQtEeXBsb20gZGFuZR8FBRF%2BL0R5cGxvbURhbmUuYXNweGRkAgIPDxYEHwQFDER5cGxvbSBwbGlraR8FBRJ%2BL0R5cGxvbVBsaWtpLmFzcHhkZAIDDw8WBB8EBQtXaWFkb21vxZtjaR8FZWQWBmYPDxYEHwQFEk1haWwgZG8gRHppZWthbmF0dR8FBRd%2BL01haWxEb0R6aWVrYW5hdHUuYXNweGRkAgEPDxYEHwQFDk9nxYJvc3plbmlhIFdVHwUFEX4vT2dsb3N6ZW5pYS5hc3B4ZGQCAg8PFgQfBAUMQWt0dWFsbm%2FFm2NpHwUFC34vTmV3cy5hc3B4ZGQCBA8PFgQfBAUSQW5raWV0eSAvIEVnemFtaW55HwVlZBYEZg8PFgQfBAUHQW5raWV0eR8FBRp%2BL1Bva2F6QW5rRWd6LmFzcHg%2FdHlwPWFua2RkAgEPDxYEHwQFCEVnemFtaW55HwUFGn4vUG9rYXpBbmtFZ3ouYXNweD90eXA9ZWd6ZGQCBQ8PFgQfBAUGV3liw7NyHwVlZBYGZg8PFgQfBAUKV3liw7NyIFctRh8FBRx%2BL1JlemVyd2FjamFQcnplZG1pb3Rvdy5hc3B4ZGQCAQ8PFgQfBAUpV3liw7NyIGJsb2vDs3cgaSBwcnplZG1pb3TDs3cgb2JpZXJhbG55Y2gfBQUZfi9XeWJvclByemVkbWlvdG93Q0MuYXNweGRkAgIPDxYEHwQFFFd5YsOzciBzcGVjamFsbm%2FFm2NpHwUFIX4vV3lib3JTcGVjamFsblNwZWNqYWxpemFjamkuYXNweGRkAgYPDxYEHwQFDFd5c3p1a2l3YXJrYR8FBRN%2BL1d5c3p1a2l3YXJrYS5hc3B4ZGQCBw8PFgQfBAUOWlVUIEUtbGVhcm5pbmcfBQUVfi9Nb29kbGVSZWRpcmVjdC5hc3B4ZGQCCA8PFgQfBAUHV3lsb2d1ah8FBQ5%2BL1d5bG9ndWouYXNweGRkAgYPZBYMAgEPDxYCHwQFD1d5YsOzciBzdHVkacOzd2RkAgMPDxYEHwQFdE5pZSBtb8W8bmEgc2nEmSB6YWxvZ293YcSHIG5hIGRhbnkgdG9rIHN0dWRpw7N3LCB6IHBvd29kdSBuaWVha3R5d25lZ28gc3RhdHVzdSBpIHVwxYJ5bmnEmWNpdSBjemFzdSBrYXJlbmNqaSBibG9rYWR5HwFoZGQCBQ8PFgIfBAUZV3liaWVyeiBraWVydW5layBzdHVkacOzd2RkAgcPEA8WBh4ORGF0YVZhbHVlRmllbGQFAklkHwgFBU5hendhHwJnZBAVAlNXeWR6aWHFgiBJbmZvcm1hdHlraSAtIGluZm9ybWF0eWthIC0gc3R1ZGlhIHBpZXJ3c3plZ28gc3RvcG5pYSAtIHN0dWRpYSBzdGFjam9uYXJuZVFXeWR6aWHFgiBJbmZvcm1hdHlraSAtIGluZm9ybWF0eWthIC0gc3R1ZGlhIGRydWdpZWdvIHN0b3BuaWEgLSBzdHVkaWEgc3RhY2pvbmFybmUVAgU0MzU0NgU4MjI0NxQrAwJnZ2RkAgkPDxYCHgxFcnJvck1lc3NhZ2UFGld5Ymllcnoga2llcnVuZWsgc3R1ZGnDs3chZGQCCw8PFgYfBAUHV3liaWVyeh4IQ3NzQ2xhc3MFCXByenljaXNrTR4EXyFTQgICZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFNmN0bDAwJGN0bDAwJFRvcE1lbnVQbGFjZUhvbGRlciR3dW1hc3Rlck1lbnVUb3AkbWVudVRvcAUwY3RsMDAkY3RsMDAkVG9wTWVudVBsYWNlSG9sZGVyJE1lbnVUb3AyJG1lbnVUb3AyBTdjdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkd3VtYXN0ZXJNZW51TGVmdCRyYWRNZW519wnOHbOxZbfQmP%2FUhOzLfUGHJVY%3D&__VIEWSTATEGENERATOR=B3016792&__EVENTVALIDATION=%2FwEWBQLaw5%2BTDgKCrfz7CgLw5Za5DQL7jZ3iAQKGn%2FjtDujJoFh%2FiOnzwkqfSgAZfEfs2myk&ctl00_ctl00_TopMenuPlaceHolder_wumasterMenuTop_menuTop_ClientState=&ctl00_ctl00_ContentPlaceHolder_wumasterMenuLeft_radMenu_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24rbKierunki={level}&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24Button1=Wybierz";
        }

        /// <summary>
        /// Returns Id's of Courses if available 
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        private List<int> GetListOfAvailableCourses(string htmlDocument)
        {
            var document = new HtmlDocument();
            document.LoadHtml(htmlDocument);
            var output = document.GetElementbyId("ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_rbKierunki");
            return output?.ChildNodes
                .Where(child => child.GetAttributeValue("value", "").IsNotNullOrEmpty())
                .Select(child => child.GetAttributeValue("value", "").ToInt())
                .ToList();
        }

        /// <summary>
        /// Generates post message to switch schedule view to whole semester
        /// </summary>
        /// <returns></returns>
        private string GenerateScheduleLinkForSemester()
        {
            var button = "Nast%C4%99pny";
            return $"ctl00_ctl00_ScriptManager1_HiddenField=&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUKLTc2ODU3NjMxMg8WAh4EZGF0YTLfAgABAAAA%2F%2F%2F%2F%2FwEAAAAAAAAADAIAAAA7V1UsIFZlcnNpb249NC4zMDMuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABZXVS5EYXRlRGlzcGxheVJhbmdlU2VtBwAAAAhfY3VycmVudAdfZGF0ZVRvDl9pbmRla3NTZW1lc3RyCl9saWN6YmFMYXQFX3R5cGUUPFJvaz5rX19CYWNraW5nRmllbGQWPExldG5pPmtfX0JhY2tpbmdGaWVsZAAAAAAEAAANDQgIIFdVLkRhdGVEaXNwbGF5UmFuZ2VTZW0rUmFuZ2VUeXBlAgAAAAgBAgAAAACA7VtfCNUIAAAAAAAAAAAQAAAAEgAAAAX9%2F%2F%2F%2FIFdVLkRhdGVEaXNwbGF5UmFuZ2VTZW0rUmFuZ2VUeXBlAQAAAAd2YWx1ZV9fAAgCAAAAAgAAAAAAAAAACxYCZg9kFgJmD2QWBGYPZBYCAgYPZBYCAgMPDxYCHgdWaXNpYmxlaGRkAgEPZBYEAgMPZBYEZg9kFgICAg8UKwACFCsAAg8WBB4LXyFEYXRhQm91bmRnHhdFbmFibGVBamF4U2tpblJlbmRlcmluZ2hkDxQrAAMUKwACDxYMHgRUZXh0BRdSb3prxYJhZCB6YWrEmcSHIChwbGFuKR4LTmF2aWdhdGVVcmwFEy9XVS9Qb2R6R29kemluLmFzcHgeBVZhbHVlBRdSb3prxYJhZCB6YWrEmcSHIChwbGFuKR4HVG9vbFRpcAUsV3nFm3dpZXRsYSBha3R1YWxueSByb3prxYJhZCB6YWrEmcSHIChwbGFuKS4eCENzc0NsYXNzBQlybUZvY3VzZWQeBF8hU0ICAmRkFCsAAg8WCB8EBQVPY2VueR8FBQ8vV1UvT2NlbnlQLmFzcHgfBgUFT2NlbnkfBwUjV3nFm3dpZXRsYSBsaXN0xJkgb3RyenltYW55Y2ggb2Nlbi5kZBQrAAIPFggfBAUMV3lsb2d1aiBtbmllHwUFEC9XVS9XeWxvZ3VqLmFzcHgfBgUMV3lsb2d1aiBtbmllHwcFIFd5bG9nb3d1amUgcHJhY293bmlrYSB6IHNlcndpc3UuZGQPFCsBA2ZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNGQWBmYPDxYMHwQFF1JvemvFgmFkIHphasSZxIcgKHBsYW4pHwUFEy9XVS9Qb2R6R29kemluLmFzcHgfBgUXUm96a8WCYWQgemFqxJnEhyAocGxhbikfBwUsV3nFm3dpZXRsYSBha3R1YWxueSByb3prxYJhZCB6YWrEmcSHIChwbGFuKS4fCAUJcm1Gb2N1c2VkHwkCAmRkAgEPDxYIHwQFBU9jZW55HwUFDy9XVS9PY2VueVAuYXNweB8GBQVPY2VueR8HBSNXecWbd2lldGxhIGxpc3TEmSBvdHJ6eW1hbnljaCBvY2VuLmRkAgIPDxYIHwQFDFd5bG9ndWogbW5pZR8FBRAvV1UvV3lsb2d1ai5hc3B4HwYFDFd5bG9ndWogbW5pZR8HBSBXeWxvZ293dWplIHByYWNvd25pa2EgeiBzZXJ3aXN1LmRkAgEPZBYCAgIPFCsAAhQrAAIPFgQfAmcfA2hkDxQrAAEUKwACDxYIHwQFB1d5bG9ndWofBQUQL1dVL1d5bG9ndWouYXNweB8GBQdXeWxvZ3VqHwcFFVd5bG9nb3d1amUgeiBzZXJ3aXN1LmRkDxQrAQFmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0ZBYCZg8PFggfBAUHV3lsb2d1ah8FBRAvV1UvV3lsb2d1ai5hc3B4HwYFB1d5bG9ndWofBwUVV3lsb2dvd3VqZSB6IHNlcndpc3UuZGQCBA9kFgRmD2QWAmYPFCsAAhQrAAIPFggeDURhdGFUZXh0RmllbGQFBFRleHQeFERhdGFOYXZpZ2F0ZVVybEZpZWxkBQtOYXZpZ2F0ZVVybB8CZx8DaGQPFCsACRQrAAIPFgQfBAUHU3R1ZGVudB8FZWQQFgxmAgECAgIDAgQCBQIGAgcCCAIJAgoCCxYMFCsAAg8WBB8EBQxEYW5lIG9nw7NsbmUfBQUNfi9XeW5pazIuYXNweGRkFCsAAg8WBB8EBRVLb250YWt0IHogRHppZWthbmF0ZW0fBQUQfi9Eemlla2FuYXQuYXNweGRkFCsAAg8WBB8EBQxUb2sgc3R1ZGnDs3cfBQURfi9Ub2tTdHVkaW93LmFzcHhkZBQrAAIPFgQfBAURUHJ6ZWJpZWcgc3R1ZGnDs3cfBQUWfi9QcnplYmllZ1N0dWRpb3cuYXNweGRkFCsAAg8WBB8EBQVPY2VueR8FBQ1%2BL09jZW55UC5hc3B4ZGQUKwACDxYEHwQFEE9jZW55IGN6xIVzdGtvd2UfBQURfi9PY2VueUN6YXN0LmFzcHhkZBQrAAIPFgQfBAULUHJvd2FkesSFY3kfBQURfi9Qcm93YWR6YWN5LmFzcHhkZBQrAAIPFgQfBAUNUGxhbiBzdHVkacOzdx8FBRJ%2BL1BsYW5TdHVkaW93LmFzcHhkZBQrAAIPFgQfBAUMUGxhbiB6YWrEmcSHHwUFEX4vUG9kekdvZHppbi5hc3B4ZGQUKwACDxYEHwQFCVN0eXBlbmRpYR8FBRB%2BL1N0eXBlbmRpYS5hc3B4ZGQUKwACDxYEHwQFD1d5ZHJ1a2kvcG9kYW5pYR8FBQ5%2BL1d5ZHJ1a2kuYXNweGRkFCsAAg8WBB8EBQhQcmFrdHlraR8FBQ9%2BL1ByYWt0eWtpLmFzcHhkZA8WDGZmZmZmZmZmZmZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAIPFgQfBAUUVHdvamUgZGFuZSBmaW5hbnNvd2UfBWVkEBYBZhYBFCsAAg8WBB8EBQxOYWxlxbxub8WbY2kfBQURfi9OYWxlem5vc2NpLmFzcHhkZA8WAWYWAQV0VGVsZXJpay5XZWIuVUkuUmFkTWVudUl0ZW0sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwACDxYEHwQFBkR5cGxvbR8FZWQQFgNmAgECAhYDFCsAAg8WBB8EBRpQcmFjYSBkeXBsb21vd2EgaW5mb3JtYWNqZR8FBQ9%2BL1ByYWNhRHlwLmFzcHhkZBQrAAIPFgQfBAULRHlwbG9tIGRhbmUfBQURfi9EeXBsb21EYW5lLmFzcHhkZBQrAAIPFgQfBAUMRHlwbG9tIHBsaWtpHwUFEn4vRHlwbG9tUGxpa2kuYXNweGRkDxYDZmZmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAg8WBB8EBQtXaWFkb21vxZtjaR8FZWQQFgNmAgECAhYDFCsAAg8WBB8EBRJNYWlsIGRvIER6aWVrYW5hdHUfBQUXfi9NYWlsRG9Eemlla2FuYXR1LmFzcHhkZBQrAAIPFgQfBAUOT2fFgm9zemVuaWEgV1UfBQURfi9PZ2xvc3plbmlhLmFzcHhkZBQrAAIPFgQfBAUMQWt0dWFsbm%2FFm2NpHwUFC34vTmV3cy5hc3B4ZGQPFgNmZmYWAQV0VGVsZXJpay5XZWIuVUkuUmFkTWVudUl0ZW0sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwACDxYEHwQFEkFua2lldHkgLyBFZ3phbWlueR8FZWQQFgJmAgEWAhQrAAIPFgQfBAUHQW5raWV0eR8FBRp%2BL1Bva2F6QW5rRWd6LmFzcHg%2FdHlwPWFua2RkFCsAAg8WBB8EBQhFZ3phbWlueR8FBRp%2BL1Bva2F6QW5rRWd6LmFzcHg%2FdHlwPWVnemRkDxYCZmYWAQV0VGVsZXJpay5XZWIuVUkuUmFkTWVudUl0ZW0sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwACDxYEHwQFBld5YsOzch8FZWQQFgNmAgECAhYDFCsAAg8WBB8EBQpXeWLDs3IgVy1GHwUFHH4vUmV6ZXJ3YWNqYVByemVkbWlvdG93LmFzcHhkZBQrAAIPFgQfBAUpV3liw7NyIGJsb2vDs3cgaSBwcnplZG1pb3TDs3cgb2JpZXJhbG55Y2gfBQUZfi9XeWJvclByemVkbWlvdG93Q0MuYXNweGRkFCsAAg8WBB8EBRRXeWLDs3Igc3BlY2phbG5vxZtjaR8FBSF%2BL1d5Ym9yU3BlY2phbG5TcGVjamFsaXphY2ppLmFzcHhkZA8WA2ZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAIPFgQfBAUMV3lzenVraXdhcmthHwUFE34vV3lzenVraXdhcmthLmFzcHhkZBQrAAIPFgQfBAUOWlVUIEUtbGVhcm5pbmcfBQUVfi9Nb29kbGVSZWRpcmVjdC5hc3B4ZGQUKwACDxYEHwQFB1d5bG9ndWofBQUOfi9XeWxvZ3VqLmFzcHhkZA8UKwEJZmZmZmZmZmZmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0ZBYSZg8PFgQfBAUHU3R1ZGVudB8FZWQWGGYPDxYEHwQFDERhbmUgb2fDs2xuZR8FBQ1%2BL1d5bmlrMi5hc3B4ZGQCAQ8PFgQfBAUVS29udGFrdCB6IER6aWVrYW5hdGVtHwUFEH4vRHppZWthbmF0LmFzcHhkZAICDw8WBB8EBQxUb2sgc3R1ZGnDs3cfBQURfi9Ub2tTdHVkaW93LmFzcHhkZAIDDw8WBB8EBRFQcnplYmllZyBzdHVkacOzdx8FBRZ%2BL1ByemViaWVnU3R1ZGlvdy5hc3B4ZGQCBA8PFgQfBAUFT2NlbnkfBQUNfi9PY2VueVAuYXNweGRkAgUPDxYEHwQFEE9jZW55IGN6xIVzdGtvd2UfBQURfi9PY2VueUN6YXN0LmFzcHhkZAIGDw8WBB8EBQtQcm93YWR6xIVjeR8FBRF%2BL1Byb3dhZHphY3kuYXNweGRkAgcPDxYEHwQFDVBsYW4gc3R1ZGnDs3cfBQUSfi9QbGFuU3R1ZGlvdy5hc3B4ZGQCCA8PFgQfBAUMUGxhbiB6YWrEmcSHHwUFEX4vUG9kekdvZHppbi5hc3B4ZGQCCQ8PFgQfBAUJU3R5cGVuZGlhHwUFEH4vU3R5cGVuZGlhLmFzcHhkZAIKDw8WBB8EBQ9XeWRydWtpL3BvZGFuaWEfBQUOfi9XeWRydWtpLmFzcHhkZAILDw8WBB8EBQhQcmFrdHlraR8FBQ9%2BL1ByYWt0eWtpLmFzcHhkZAIBDw8WBB8EBRRUd29qZSBkYW5lIGZpbmFuc293ZR8FZWQWAmYPDxYEHwQFDE5hbGXFvG5vxZtjaR8FBRF%2BL05hbGV6bm9zY2kuYXNweGRkAgIPDxYEHwQFBkR5cGxvbR8FZWQWBmYPDxYEHwQFGlByYWNhIGR5cGxvbW93YSBpbmZvcm1hY2plHwUFD34vUHJhY2FEeXAuYXNweGRkAgEPDxYEHwQFC0R5cGxvbSBkYW5lHwUFEX4vRHlwbG9tRGFuZS5hc3B4ZGQCAg8PFgQfBAUMRHlwbG9tIHBsaWtpHwUFEn4vRHlwbG9tUGxpa2kuYXNweGRkAgMPDxYEHwQFC1dpYWRvbW%2FFm2NpHwVlZBYGZg8PFgQfBAUSTWFpbCBkbyBEemlla2FuYXR1HwUFF34vTWFpbERvRHppZWthbmF0dS5hc3B4ZGQCAQ8PFgQfBAUOT2fFgm9zemVuaWEgV1UfBQURfi9PZ2xvc3plbmlhLmFzcHhkZAICDw8WBB8EBQxBa3R1YWxub8WbY2kfBQULfi9OZXdzLmFzcHhkZAIEDw8WBB8EBRJBbmtpZXR5IC8gRWd6YW1pbnkfBWVkFgRmDw8WBB8EBQdBbmtpZXR5HwUFGn4vUG9rYXpBbmtFZ3ouYXNweD90eXA9YW5rZGQCAQ8PFgQfBAUIRWd6YW1pbnkfBQUafi9Qb2thekFua0Vnei5hc3B4P3R5cD1lZ3pkZAIFDw8WBB8EBQZXeWLDs3IfBWVkFgZmDw8WBB8EBQpXeWLDs3IgVy1GHwUFHH4vUmV6ZXJ3YWNqYVByemVkbWlvdG93LmFzcHhkZAIBDw8WBB8EBSlXeWLDs3IgYmxva8OzdyBpIHByemVkbWlvdMOzdyBvYmllcmFsbnljaB8FBRl%2BL1d5Ym9yUHJ6ZWRtaW90b3dDQy5hc3B4ZGQCAg8PFgQfBAUUV3liw7NyIHNwZWNqYWxub8WbY2kfBQUhfi9XeWJvclNwZWNqYWxuU3BlY2phbGl6YWNqaS5hc3B4ZGQCBg8PFgQfBAUMV3lzenVraXdhcmthHwUFE34vV3lzenVraXdhcmthLmFzcHhkZAIHDw8WBB8EBQ5aVVQgRS1sZWFybmluZx8FBRV%2BL01vb2RsZVJlZGlyZWN0LmFzcHhkZAIIDw8WBB8EBQdXeWxvZ3VqHwUFDn4vV3lsb2d1ai5hc3B4ZGQCBg9kFhgCAw88KwALAQAPFgIfAWhkZAIFDw8WBB8EBd8CPHRhYmxlIGJvcmRlcj0iMCI%2BDQoJPHRyPg0KCQk8dGQgc3R5bGU9IndpZHRoOjIwcHg7aGVpZ2h0OjIwcHg7YmFja2dyb3VuZC1jb2xvcjojMDAwMEZGOyI%2BPC90ZD48dGQ%2BIC0gZWd6YW1pbiAoZSk8L3RkPg0KCTwvdHI%2BPHRyPg0KCQk8dGQgc3R5bGU9IndpZHRoOjIwcHg7aGVpZ2h0OjIwcHg7YmFja2dyb3VuZC1jb2xvcjojRkYwMDAwOyI%2BPC90ZD48dGQ%2BIC0gb2R3b8WCYW5lIChvKTwvdGQ%2BDQoJPC90cj48dHI%2BDQoJCTx0ZCBzdHlsZT0id2lkdGg6MjBweDtoZWlnaHQ6MjBweDtiYWNrZ3JvdW5kLWNvbG9yOiMwMDk5MDA7Ij48L3RkPjx0ZD4gLSByZWt0b3Jza2llIChyKTwvdGQ%2BDQoJPC90cj4NCjwvdGFibGU%2BHwFoZGQCCQ9kFgQCAQ8PFgIfBAUfR3J1cHkgYmV6IHphcGxhbm93YW55Y2ggemFqxJnEh2RkAgMPPCsADgIAFCsAAmQXAAEWAhYLDwIFFCsABRQrAAUWAh4KSGVhZGVyVGV4dAUJUHJ6ZWRtaW90ZGRkBQlQcnplZG1pb3QUKwAFFgIfDAULUHJvd2FkesSFY3lkZGQFClByb3dhZHphY3kUKwAFFgIfDAUNRm9ybWEgemFqxJnEh2RkZAUKRm9ybWFaYWplYxQrAAUWAh8MBQ1MaWN6YmEgbGVrY2ppZGRkBQxMaWN6YmFHb2R6aW4UKwAFFgIfDAUNRm9ybWEgemFsaWN6LmRkZAUPRm9ybWFaYWxpY3plbmlhZGUUKwAACyl6VGVsZXJpay5XZWIuVUkuR3JpZENoaWxkTG9hZE1vZGUsIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQBPCsABwALKXVUZWxlcmlrLldlYi5VSS5HcmlkRWRpdE1vZGUsIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQBFgIeBF9lZnNkZBYEHgpEYXRhTWVtYmVyZR4EX2hsbQsrBAFkZmQCEQ8QZA8WA2YCAQICFgMFCER6aWVubmllBQpUeWdvZG5pb3dvBQxTZW1lc3RyYWxuaWUWAQICZAIVDw8WBB4MU2VsZWN0ZWREYXRlBgDYDgBIetWIHhFfc2tpcE1NVmFsaWRhdGlvbmhkFgRmDxQrAAgPFhIeDUxhYmVsQ3NzQ2xhc3MFB3JpTGFiZWweDEVtcHR5TWVzc2FnZWUfBAUTMjAxOC0wMi0yMy0wMC0wMC0wMB4HTWluRGF0ZQYAgEwEt7WqCB4EU2tpbgUHRGVmYXVsdB8RaB8DaB4RRW5hYmxlQXJpYVN1cHBvcnRoHgdNYXhEYXRlBgBAcW%2BxPjEJZBYGHgVXaWR0aBsAAAAAAABZQAcAAAAfCAURcmlUZXh0Qm94IHJpSG92ZXIfCQKCAhYGHxgbAAAAAAAAWUAHAAAAHwgFEXJpVGV4dEJveCByaUVycm9yHwkCggIWBh8YGwAAAAAAAFlABwAAAB8IBRNyaVRleHRCb3ggcmlGb2N1c2VkHwkCggIWBh8YGwAAAAAAAFlABwAAAB8IBRNyaVRleHRCb3ggcmlFbmFibGVkHwkCggIWBh8YGwAAAAAAAFlABwAAAB8IBRRyaVRleHRCb3ggcmlEaXNhYmxlZB8JAoICFgYfGBsAAAAAAABZQAcAAAAfCAURcmlUZXh0Qm94IHJpRW1wdHkfCQKCAhYGHxgbAAAAAAAAWUAHAAAAHwgFEHJpVGV4dEJveCByaVJlYWQfCQKCAmQCAg8UKwANDxYQBRJGYXN0TmF2aWdhdGlvblN0ZXACDAUETWluRAYAgEwEt7WqCAUPUmVuZGVySW52aXNpYmxlZwUNU2VsZWN0ZWREYXRlcw8FkAFUZWxlcmlrLldlYi5VSS5DYWxlbmRhci5Db2xsZWN0aW9ucy5EYXRlVGltZUNvbGxlY3Rpb24sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwAABQRGb2NEBgBA02FQetUIBRFFbmFibGVNdWx0aVNlbGVjdGgFC1NwZWNpYWxEYXlzDwWTAVRlbGVyaWsuV2ViLlVJLkNhbGVuZGFyLkNvbGxlY3Rpb25zLkNhbGVuZGFyRGF5Q29sbGVjdGlvbiwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAAFBE1heEQGAIAHReg9MQkPFgYfFmgfFQUHRGVmYXVsdB8DaGRkFgQfCAULcmNNYWluVGFibGUfCQICFgQfCAUMcmNPdGhlck1vbnRoHwkCAmQWBB8IBQpyY1NlbGVjdGVkHwkCAmQWBB8IBQpyY0Rpc2FibGVkHwkCAhYEHwgFDHJjT3V0T2ZSYW5nZR8JAgIWBB8IBQlyY1dlZWtlbmQfCQICFgQfCAUHcmNIb3Zlch8JAgIWBB8IBTFSYWRDYWxlbmRhck1vbnRoVmlldyBSYWRDYWxlbmRhck1vbnRoVmlld19EZWZhdWx0HwkCAhYEHwgFCXJjVmlld1NlbB8JAgJkAhcPDxYEHxAGANgOAEh61YgfEWhkFgRmDxQrAAgPFhIfEgUHcmlMYWJlbB8TZR8EBRMyMDE4LTAyLTIzLTAwLTAwLTAwHxQGAIBMBLe1qggfFQUHRGVmYXVsdB8RaB8DaB8WaB8XBgBAcW%2BxPjEJZBYGHxgbAAAAAAAAWUAHAAAAHwgFEXJpVGV4dEJveCByaUhvdmVyHwkCggIWBh8YGwAAAAAAAFlABwAAAB8IBRFyaVRleHRCb3ggcmlFcnJvch8JAoICFgYfGBsAAAAAAABZQAcAAAAfCAUTcmlUZXh0Qm94IHJpRm9jdXNlZB8JAoICFgYfGBsAAAAAAABZQAcAAAAfCAUTcmlUZXh0Qm94IHJpRW5hYmxlZB8JAoICFgYfGBsAAAAAAABZQAcAAAAfCAUUcmlUZXh0Qm94IHJpRGlzYWJsZWQfCQKCAhYGHxgbAAAAAAAAWUAHAAAAHwgFEXJpVGV4dEJveCByaUVtcHR5HwkCggIWBh8YGwAAAAAAAFlABwAAAB8IBRByaVRleHRCb3ggcmlSZWFkHwkCggJkAgIPFCsADQ8WEAUSRmFzdE5hdmlnYXRpb25TdGVwAgwFBE1pbkQGAIBMBLe1qggFD1JlbmRlckludmlzaWJsZWcFDVNlbGVjdGVkRGF0ZXMPBZABVGVsZXJpay5XZWIuVUkuQ2FsZW5kYXIuQ29sbGVjdGlvbnMuRGF0ZVRpbWVDb2xsZWN0aW9uLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAAUERm9jRAYAQNNhUHrVCAURRW5hYmxlTXVsdGlTZWxlY3RoBQtTcGVjaWFsRGF5cw8FkwFUZWxlcmlrLldlYi5VSS5DYWxlbmRhci5Db2xsZWN0aW9ucy5DYWxlbmRhckRheUNvbGxlY3Rpb24sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwAABQRNYXhEBgCAB0XoPTEJDxYGHxZoHxUFB0RlZmF1bHQfA2hkZBYEHwgFC3JjTWFpblRhYmxlHwkCAhYEHwgFDHJjT3RoZXJNb250aB8JAgJkFgQfCAUKcmNTZWxlY3RlZB8JAgJkFgQfCAUKcmNEaXNhYmxlZB8JAgIWBB8IBQxyY091dE9mUmFuZ2UfCQICFgQfCAUJcmNXZWVrZW5kHwkCAhYEHwgFB3JjSG92ZXIfCQICFgQfCAUxUmFkQ2FsZW5kYXJNb250aFZpZXcgUmFkQ2FsZW5kYXJNb250aFZpZXdfRGVmYXVsdB8JAgIWBB8IBQlyY1ZpZXdTZWwfCQICZAIZDw8WAh8EBQdGaWx0cnVqZGQCGw8QDxYEHwQFClBsYW4gc2VzamkfAWhkZGRkAh0PFgYeBXZhbHVlBQhXeWRydWt1ah4FY2xhc3MFCXByenljaXNrTR8BaGQCHw8WBh8ZBRtXeWRydWt1aiBwbyBkbmlhY2ggdHlnb2RuaWEfGgUJcHJ6eWNpc2tNHwFoZAIhDxYGHxkFIVBvYmllcnogcGxhbiB3IGZvcm1hY2llIGlDYWxlbmRhch8aBQlwcnp5Y2lza00fAWhkAiMPFgYfGQUwUG9iaWVyeiBwbGFuIHcgZm9ybWFjaWUgaUNhbGVuZGFyIGRsYSBHcm91cCBXaXNlHxoFCXByenljaXNrTR8BaGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgkFNmN0bDAwJGN0bDAwJFRvcE1lbnVQbGFjZUhvbGRlciR3dW1hc3Rlck1lbnVUb3AkbWVudVRvcAUwY3RsMDAkY3RsMDAkVG9wTWVudVBsYWNlSG9sZGVyJE1lbnVUb3AyJG1lbnVUb3AyBTdjdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkd3VtYXN0ZXJNZW51TGVmdCRyYWRNZW51BUBjdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkUmlnaHRDb250ZW50UGxhY2VIb2xkZXIkcmFkRGF0YU9kBUljdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkUmlnaHRDb250ZW50UGxhY2VIb2xkZXIkcmFkRGF0YU9kJGNhbGVuZGFyBUljdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkUmlnaHRDb250ZW50UGxhY2VIb2xkZXIkcmFkRGF0YU9kJGNhbGVuZGFyBUBjdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkUmlnaHRDb250ZW50UGxhY2VIb2xkZXIkcmFkRGF0YURvBUljdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkUmlnaHRDb250ZW50UGxhY2VIb2xkZXIkcmFkRGF0YURvJGNhbGVuZGFyBUljdGwwMCRjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIkUmlnaHRDb250ZW50UGxhY2VIb2xkZXIkcmFkRGF0YURvJGNhbGVuZGFyapdS4pX0BcPg%2FsqBQSD7w6RS7l8%3D&__VIEWSTATEGENERATOR=C842751A&__EVENTVALIDATION=%2FwEWDwKtxfLxAgKBq8OvBwKjovauBAKn6MnOBgLYtMHMCgLnprz5BQL%2F%2B4isBALSn6sTAoud7qYFApy0vaUKAtWxoogIArD%2FosELApz3uMsEApbL89oHAvieuT0Rt20zGOl3rRab7lZxJwebn%2FKbaA%3D%3D&ctl00_ctl00_TopMenuPlaceHolder_wumasterMenuTop_menuTop_ClientState=&ctl00_ctl00_ContentPlaceHolder_wumasterMenuLeft_radMenu_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24butN={button}&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24rbJak=Semestralnie&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataOd=2018-02-23&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataOd%24dateInput=23.02.2018&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_dateInput_ClientState=%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%222018-02-23-00-00-00%22%2C%22valueAsString%22%3A%222018-02-23-00-00-00%22%2C%22minDateStr%22%3A%221980-01-01-00-00-00%22%2C%22maxDateStr%22%3A%222099-12-31-00-00-00%22%7D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_calendar_SD=%5B%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_calendar_AD=%5B%5B1980%2C1%2C1%5D%2C%5B2099%2C12%2C30%5D%2C%5B2018%2C2%2C23%5D%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataDo=2018-02-23&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataDo%24dateInput=23.02.2018&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_dateInput_ClientState=%7B%22enabled%22%3Atrue%2C%22emptyMessage%22%3A%22%22%2C%22validationText%22%3A%222018-02-23-00-00-00%22%2C%22valueAsString%22%3A%222018-02-23-00-00-00%22%2C%22minDateStr%22%3A%221980-01-01-00-00-00%22%2C%22maxDateStr%22%3A%222099-12-31-00-00-00%22%7D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_calendar_SD=%5B%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_calendar_AD=%5B%5B1980%2C1%2C1%5D%2C%5B2099%2C12%2C30%5D%2C%5B2018%2C2%2C23%5D%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24hid_Temp=";
        }

        /// <summary>
        /// Returns list of <see cref="DayViewModel"/> from Schedule HTML
        /// </summary>
        /// <param name="siteHTML"></param>
        /// <returns></returns>
        private List<DayViewModel> GenerateDaysFromSiteDocument(string siteHTML)
        {
            if (string.IsNullOrWhiteSpace(siteHTML))
            {
                return null;
            }

            var result = new List<DayViewModel>();

            // Get HTML table with all classes
            var tableWithClasses = Regex.Matches(siteHTML,
                "<table.*?\\\"ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_dgDane\\\".*?><tbody>.*?</tbody></table>");

            if (tableWithClasses.Count == 0)
            {
                return null;
            }

            // Get record form table
            var records = Regex.Matches(tableWithClasses[0].Value,
                "<tr class=\\\"gridDane\\\">(?:<td(?:.*?)>?(?:(?:<a.*?>)*(.*?)(?:</a>)*)</td>?)</tr>");


            // Generate all classes from records
            foreach (var record in records)
            {
                var recordData = Regex.Matches(record.ToString(),
                    "(?:<td.*?>(?:(?:<font.*?>)*(?:<a.*?>)*(.*?)(?:</a>)*(?:</font>)*)</td>?)");

                // Prepare data
                var date = recordData.getValueAt(0).Split(' ')[0].Split('.');
                var startTime = recordData.getValueAt(1).Split(':');
                var endTime = recordData.getValueAt(2).Split(':');

                // Generate DateTime
                var classDateTime = new DateTime(date[2].ToInt(), date[1].ToInt(), date[0].ToInt());
                var classStartDateTime = new DateTime(date[2].ToInt(), date[1].ToInt(), date[0].ToInt(), startTime[0].ToInt(), startTime[1].ToInt(), 0);
                var classEndDateTime = new DateTime(date[2].ToInt(), date[1].ToInt(), date[0].ToInt(), endTime[0].ToInt(), endTime[1].ToInt(), 0);

                // If there is no day with this date...
                if (result.Where(d => d.Date == classDateTime.OnlyDate()).Count() == 0)
                {
                    // add new day
                    result.Add(new DayViewModel() { Date = classDateTime.OnlyDate() });
                }

                // add class to last added day
                result.Last().Courses.Add(new ClassViewModel()
                {
                    StartTime = classStartDateTime,
                    EndTime = classEndDateTime,
                    ClassroomID = recordData.getValueAt(ClassRecordData.Place),
                    TeacherName = recordData.getValueAt(ClassRecordData.Teacher),
                    CourseName = $"{recordData.getValueAt(ClassRecordData.Course)} ({recordData.getValueAt(ClassRecordData.CourseType)})",
                    Status = recordData.getValueAt(recordData.Count - 1),
                });

            }

            return result;
        }
    }
}
