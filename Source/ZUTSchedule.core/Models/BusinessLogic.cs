﻿using System;
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
            _communicationService = IoC.CommunicationService;
        }

        /// <summary>
        /// Attempt to login user to e-Dziekanat system
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        public async Task<bool> LoginAsync(Credential creditials)
        {
            try
            {
                var result = await _communicationService.SendAsync(IoC.Settings.loginURL, GenerateLoginContent(creditials));
                return LoggedIn(result);
            }
            catch(HttpRequestException ex)
            {
                Logger.Log($"Login attempt failed \n {ex.Message} \n\n {ex.StackTrace}", Logger.LogLevel.Error);
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
            return new StringContent($"ctl00_ctl00_ScriptManager1_HiddenField=&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUKMTc3NTQ1OTc2NA8WAh4DaGFzZRYCZg9kFgJmD2QWAgIBD2QWBAIDD2QWAgIBD2QWAgIBD2QWAgICDxQrAAIUKwACDxYEHgtfIURhdGFCb3VuZGceF0VuYWJsZUFqYXhTa2luUmVuZGVyaW5naGRkZGQCBA9kFgICAQ9kFhICAQ8WAh4JaW5uZXJodG1sBSZlLUR6aWVrYW5hdDwhLS0gc3RhdHVzOiA1NjE0NTAxMjYgLS0%2BIGQCDQ8PFgIeBE1vZGULKiVTeXN0ZW0uV2ViLlVJLldlYkNvbnRyb2xzLlRleHRCb3hNb2RlAmRkAhUPDxYEHgRUZXh0BRlPZHp5c2tpd2FuaWUgaGFzxYJhPGJyIC8%2BHgdWaXNpYmxlaGRkAhcPZBYCAgMPEGQPFgJmAgEWAgURc3R1ZGVudC9kb2t0b3JhbnQFCGR5ZGFrdHlrFgFmZAIZD2QWBAIBDw8WAh8FBTQ8YnIgLz5MdWIgemFsb2d1aiBzacSZIGpha28gc3R1ZGVudCBwcnpleiBPZmZpY2UzNjU6ZGQCAw8PFgIfBQUIUHJ6ZWpkxbpkZAIbDw8WBB8FBRhTZXJ3aXMgQWJzb2x3ZW50w7N3PGJyLz4fBmhkZAIfDw8WAh8GaGRkAiEPDxYCHwZoZBYGAgEPDxYCHwVkZGQCAw8PFgIfBQUGQW51bHVqZGQCBQ8PFgIfBQUHUG9iaWVyemRkAiMPDxYCHwUFI1fFgsSFY3ogcmVrbGFtxJkgYXBsaWthY2ppIG1vYmlsbmVqZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFSmN0bDAwJGN0bDAwJFRvcE1lbnVQbGFjZUhvbGRlciRUb3BNZW51Q29udGVudFBsYWNlSG9sZGVyJE1lbnVUb3AzJG1lbnVUb3Az38BzqfstGliCA7rUUjKrKnlhZpc%3D&__VIEWSTATEGENERATOR=7D6A02AE&ctl00_ctl00_TopMenuPlaceHolder_TopMenuContentPlaceHolder_MenuTop3_menuTop3_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtIdent={userCredential.UserName}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24txtHaslo={userCredential.Password.Unsecure()}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24rbKto={IoC.Settings.Typ}&ctl00%24ctl00%24ContentPlaceHolder%24MiddleContentPlaceHolder%24butLoguj=Zaloguj",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Checks if user is logged in
        /// </summary>
        /// <param name="siteHTML"></param>
        /// <returns></returns>
        private bool LoggedIn(string siteHTML)
        {
            if (string.IsNullOrWhiteSpace(siteHTML))
            {
                return false;
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
                if(IoC.Settings.IsUserLoggedIn == false)
                {
                    await LoginAsync(IoC.Settings.UserCredential);
                }

                // Go to schedule full schedule view
                var modeToSemesterViewContent = new StringContent("ctl00_ctl00_ScriptManager1_HiddenField=&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUKLTc2ODU3NjMxMg8WBB4EZGF0YTLfAgABAAAA%2F%2F%2F%2F%2FwEAAAAAAAAADAIAAAA7V1UsIFZlcnNpb249NC4yOTIuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABZXVS5EYXRlRGlzcGxheVJhbmdlU2VtBwAAAAhfY3VycmVudAdfZGF0ZVRvDl9pbmRla3NTZW1lc3RyCl9saWN6YmFMYXQFX3R5cGUUPFJvaz5rX19CYWNraW5nRmllbGQWPExldG5pPmtfX0JhY2tpbmdGaWVsZAAAAAAEAAANDQgIIFdVLkRhdGVEaXNwbGF5UmFuZ2VTZW0rUmFuZ2VUeXBlAgAAAAgBAgAAAAAAlaSpL9WIAAAAAAAAAAAAAAAAEgAAAAX9%2F%2F%2F%2FIFdVLkRhdGVEaXNwbGF5UmFuZ2VTZW0rUmFuZ2VUeXBlAQAAAAd2YWx1ZV9fAAgCAAAAAQAAAAAAAAAACx4DaWRzFRUFMjMzOTIGMTExMzc4BjU5MDU5MgUyMzM5MgYxMTEzNzcGNzMyNDI3BTIzMTA2BjExMTM4MAY3NDQxMzUFMjMxMDYGMTExMzgwBjU5MDM4NwUyMzMwMgYxMDk2NjcGNTkwNzE0BTIzMzAyBjEwOTY2NQY1OTA0MjQFMjMzMDIGMTA5NjY1Bjc0NDEzNhYCZg9kFgJmD2QWBGYPZBYCAgYPZBYCAgMPDxYCHgdWaXNpYmxlaGRkAgEPZBYEAgMPZBYEZg9kFgICAg8UKwACFCsAAg8WBB4LXyFEYXRhQm91bmRnHhdFbmFibGVBamF4U2tpblJlbmRlcmluZ2hkDxQrAAMUKwACDxYMHgRUZXh0BRdSb3prxYJhZCB6YWrEmcSHIChwbGFuKR4LTmF2aWdhdGVVcmwFEy9XVS9Qb2R6R29kemluLmFzcHgeBVZhbHVlBRdSb3prxYJhZCB6YWrEmcSHIChwbGFuKR4HVG9vbFRpcAUsV3nFm3dpZXRsYSBha3R1YWxueSByb3prxYJhZCB6YWrEmcSHIChwbGFuKS4eCENzc0NsYXNzBQlybUZvY3VzZWQeBF8hU0ICAmRkFCsAAg8WCB8FBQVPY2VueR8GBQ8vV1UvT2NlbnlQLmFzcHgfBwUFT2NlbnkfCAUjV3nFm3dpZXRsYSBsaXN0xJkgb3RyenltYW55Y2ggb2Nlbi5kZBQrAAIPFggfBQUMV3lsb2d1aiBtbmllHwYFEC9XVS9XeWxvZ3VqLmFzcHgfBwUMV3lsb2d1aiBtbmllHwgFIFd5bG9nb3d1amUgcHJhY293bmlrYSB6IHNlcndpc3UuZGQPFCsBA2ZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNGQWBmYPDxYMHwUFF1JvemvFgmFkIHphasSZxIcgKHBsYW4pHwYFEy9XVS9Qb2R6R29kemluLmFzcHgfBwUXUm96a8WCYWQgemFqxJnEhyAocGxhbikfCAUsV3nFm3dpZXRsYSBha3R1YWxueSByb3prxYJhZCB6YWrEmcSHIChwbGFuKS4fCQUJcm1Gb2N1c2VkHwoCAmRkAgEPDxYIHwUFBU9jZW55HwYFDy9XVS9PY2VueVAuYXNweB8HBQVPY2VueR8IBSNXecWbd2lldGxhIGxpc3TEmSBvdHJ6eW1hbnljaCBvY2VuLmRkAgIPDxYIHwUFDFd5bG9ndWogbW5pZR8GBRAvV1UvV3lsb2d1ai5hc3B4HwcFDFd5bG9ndWogbW5pZR8IBSBXeWxvZ293dWplIHByYWNvd25pa2EgeiBzZXJ3aXN1LmRkAgEPZBYCAgIPFCsAAhQrAAIPFgQfA2cfBGhkDxQrAAEUKwACDxYIHwUFB1d5bG9ndWofBgUQL1dVL1d5bG9ndWouYXNweB8HBQdXeWxvZ3VqHwgFFVd5bG9nb3d1amUgeiBzZXJ3aXN1LmRkDxQrAQFmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0ZBYCZg8PFggfBQUHV3lsb2d1ah8GBRAvV1UvV3lsb2d1ai5hc3B4HwcFB1d5bG9ndWofCAUVV3lsb2dvd3VqZSB6IHNlcndpc3UuZGQCBA9kFgRmD2QWAmYPFCsAAhQrAAIPFggeDURhdGFUZXh0RmllbGQFBFRleHQeFERhdGFOYXZpZ2F0ZVVybEZpZWxkBQtOYXZpZ2F0ZVVybB8DZx8EaGQPFCsACRQrAAIPFgQfBQUHU3R1ZGVudB8GZWQQFgtmAgECAgIDAgQCBQIGAgcCCAIJAgoWCxQrAAIPFgQfBQUMRGFuZSBvZ8OzbG5lHwYFDX4vV3luaWsyLmFzcHhkZBQrAAIPFgQfBQUVS29udGFrdCB6IER6aWVrYW5hdGVtHwYFEH4vRHppZWthbmF0LmFzcHhkZBQrAAIPFgQfBQUMVG9rIHN0dWRpw7N3HwYFEX4vVG9rU3R1ZGlvdy5hc3B4ZGQUKwACDxYEHwUFEVByemViaWVnIHN0dWRpw7N3HwYFFn4vUHJ6ZWJpZWdTdHVkaW93LmFzcHhkZBQrAAIPFgQfBQUFT2NlbnkfBgUNfi9PY2VueVAuYXNweGRkFCsAAg8WBB8FBRBPY2VueSBjesSFc3Rrb3dlHwYFEX4vT2NlbnlDemFzdC5hc3B4ZGQUKwACDxYEHwUFC1Byb3dhZHrEhWN5HwYFEX4vUHJvd2FkemFjeS5hc3B4ZGQUKwACDxYEHwUFDVBsYW4gc3R1ZGnDs3cfBgUSfi9QbGFuU3R1ZGlvdy5hc3B4ZGQUKwACDxYEHwUFDFBsYW4gemFqxJnEhx8GBRF%2BL1BvZHpHb2R6aW4uYXNweGRkFCsAAg8WBB8FBQlTdHlwZW5kaWEfBgUQfi9TdHlwZW5kaWEuYXNweGRkFCsAAg8WBB8FBQ9XeWRydWtpL3BvZGFuaWEfBgUOfi9XeWRydWtpLmFzcHhkZA8WC2ZmZmZmZmZmZmZmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAg8WBB8FBRRUd29qZSBkYW5lIGZpbmFuc293ZR8GZWQQFgFmFgEUKwACDxYEHwUFDE5hbGXFvG5vxZtjaR8GBRF%2BL05hbGV6bm9zY2kuYXNweGRkDxYBZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAIPFgQfBQUGRHlwbG9tHwZlZBAWA2YCAQICFgMUKwACDxYEHwUFGlByYWNhIGR5cGxvbW93YSBpbmZvcm1hY2plHwYFD34vUHJhY2FEeXAuYXNweGRkFCsAAg8WBB8FBQtEeXBsb20gZGFuZR8GBRF%2BL0R5cGxvbURhbmUuYXNweGRkFCsAAg8WBB8FBQxEeXBsb20gcGxpa2kfBgUSfi9EeXBsb21QbGlraS5hc3B4ZGQPFgNmZmYWAQV0VGVsZXJpay5XZWIuVUkuUmFkTWVudUl0ZW0sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDQUKwACDxYEHwUFC1dpYWRvbW%2FFm2NpHwZlZBAWA2YCAQICFgMUKwACDxYEHwUFEk1haWwgZG8gRHppZWthbmF0dR8GBRd%2BL01haWxEb0R6aWVrYW5hdHUuYXNweGRkFCsAAg8WBB8FBQ5PZ8WCb3N6ZW5pYSBXVR8GBRF%2BL09nbG9zemVuaWEuYXNweGRkFCsAAg8WBB8FBQxBa3R1YWxub8WbY2kfBgULfi9OZXdzLmFzcHhkZA8WA2ZmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAIPFgQfBQUSQW5raWV0eSAvIEVnemFtaW55HwZlZBAWAmYCARYCFCsAAg8WBB8FBQdBbmtpZXR5HwYFGn4vUG9rYXpBbmtFZ3ouYXNweD90eXA9YW5rZGQUKwACDxYEHwUFCEVnemFtaW55HwYFGn4vUG9rYXpBbmtFZ3ouYXNweD90eXA9ZWd6ZGQPFgJmZhYBBXRUZWxlcmlrLldlYi5VSS5SYWRNZW51SXRlbSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAIPFgQfBQUGV3liw7NyHwZlZBAWA2YCAQICFgMUKwACDxYEHwUFCld5YsOzciBXLUYfBgUcfi9SZXplcndhY2phUHJ6ZWRtaW90b3cuYXNweGRkFCsAAg8WBB8FBSlXeWLDs3IgYmxva8OzdyBpIHByemVkbWlvdMOzdyBvYmllcmFsbnljaB8GBRl%2BL1d5Ym9yUHJ6ZWRtaW90b3dDQy5hc3B4ZGQUKwACDxYEHwUFFFd5YsOzciBzcGVjamFsbm%2FFm2NpHwYFIX4vV3lib3JTcGVjamFsblNwZWNqYWxpemFjamkuYXNweGRkDxYDZmZmFgEFdFRlbGVyaWsuV2ViLlVJLlJhZE1lbnVJdGVtLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAg8WBB8FBQxXeXN6dWtpd2Fya2EfBgUTfi9XeXN6dWtpd2Fya2EuYXNweGRkFCsAAg8WBB8FBQ5aVVQgRS1sZWFybmluZx8GBRV%2BL01vb2RsZVJlZGlyZWN0LmFzcHhkZBQrAAIPFgQfBQUHV3lsb2d1ah8GBQ5%2BL1d5bG9ndWouYXNweGRkDxQrAQlmZmZmZmZmZmYWAQV0VGVsZXJpay5XZWIuVUkuUmFkTWVudUl0ZW0sIFRlbGVyaWsuV2ViLlVJLCBWZXJzaW9uPTIwMTIuMy4xMjA1LjM1LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPTEyMWZhZTc4MTY1YmEzZDRkFhJmDw8WBB8FBQdTdHVkZW50HwZlZBYWZg8PFgQfBQUMRGFuZSBvZ8OzbG5lHwYFDX4vV3luaWsyLmFzcHhkZAIBDw8WBB8FBRVLb250YWt0IHogRHppZWthbmF0ZW0fBgUQfi9Eemlla2FuYXQuYXNweGRkAgIPDxYEHwUFDFRvayBzdHVkacOzdx8GBRF%2BL1Rva1N0dWRpb3cuYXNweGRkAgMPDxYEHwUFEVByemViaWVnIHN0dWRpw7N3HwYFFn4vUHJ6ZWJpZWdTdHVkaW93LmFzcHhkZAIEDw8WBB8FBQVPY2VueR8GBQ1%2BL09jZW55UC5hc3B4ZGQCBQ8PFgQfBQUQT2NlbnkgY3rEhXN0a293ZR8GBRF%2BL09jZW55Q3phc3QuYXNweGRkAgYPDxYEHwUFC1Byb3dhZHrEhWN5HwYFEX4vUHJvd2FkemFjeS5hc3B4ZGQCBw8PFgQfBQUNUGxhbiBzdHVkacOzdx8GBRJ%2BL1BsYW5TdHVkaW93LmFzcHhkZAIIDw8WBB8FBQxQbGFuIHphasSZxIcfBgURfi9Qb2R6R29kemluLmFzcHhkZAIJDw8WBB8FBQlTdHlwZW5kaWEfBgUQfi9TdHlwZW5kaWEuYXNweGRkAgoPDxYEHwUFD1d5ZHJ1a2kvcG9kYW5pYR8GBQ5%2BL1d5ZHJ1a2kuYXNweGRkAgEPDxYEHwUFFFR3b2plIGRhbmUgZmluYW5zb3dlHwZlZBYCZg8PFgQfBQUMTmFsZcW8bm%2FFm2NpHwYFEX4vTmFsZXpub3NjaS5hc3B4ZGQCAg8PFgQfBQUGRHlwbG9tHwZlZBYGZg8PFgQfBQUaUHJhY2EgZHlwbG9tb3dhIGluZm9ybWFjamUfBgUPfi9QcmFjYUR5cC5hc3B4ZGQCAQ8PFgQfBQULRHlwbG9tIGRhbmUfBgURfi9EeXBsb21EYW5lLmFzcHhkZAICDw8WBB8FBQxEeXBsb20gcGxpa2kfBgUSfi9EeXBsb21QbGlraS5hc3B4ZGQCAw8PFgQfBQULV2lhZG9tb8WbY2kfBmVkFgZmDw8WBB8FBRJNYWlsIGRvIER6aWVrYW5hdHUfBgUXfi9NYWlsRG9Eemlla2FuYXR1LmFzcHhkZAIBDw8WBB8FBQ5PZ8WCb3N6ZW5pYSBXVR8GBRF%2BL09nbG9zemVuaWEuYXNweGRkAgIPDxYEHwUFDEFrdHVhbG5vxZtjaR8GBQt%2BL05ld3MuYXNweGRkAgQPDxYEHwUFEkFua2lldHkgLyBFZ3phbWlueR8GZWQWBGYPDxYEHwUFB0Fua2lldHkfBgUafi9Qb2thekFua0Vnei5hc3B4P3R5cD1hbmtkZAIBDw8WBB8FBQhFZ3phbWlueR8GBRp%2BL1Bva2F6QW5rRWd6LmFzcHg%2FdHlwPWVnemRkAgUPDxYEHwUFBld5YsOzch8GZWQWBmYPDxYEHwUFCld5YsOzciBXLUYfBgUcfi9SZXplcndhY2phUHJ6ZWRtaW90b3cuYXNweGRkAgEPDxYEHwUFKVd5YsOzciBibG9rw7N3IGkgcHJ6ZWRtaW90w7N3IG9iaWVyYWxueWNoHwYFGX4vV3lib3JQcnplZG1pb3Rvd0NDLmFzcHhkZAICDw8WBB8FBRRXeWLDs3Igc3BlY2phbG5vxZtjaR8GBSF%2BL1d5Ym9yU3BlY2phbG5TcGVjamFsaXphY2ppLmFzcHhkZAIGDw8WBB8FBQxXeXN6dWtpd2Fya2EfBgUTfi9XeXN6dWtpd2Fya2EuYXNweGRkAgcPDxYEHwUFDlpVVCBFLWxlYXJuaW5nHwYFFX4vTW9vZGxlUmVkaXJlY3QuYXNweGRkAggPDxYEHwUFB1d5bG9ndWofBgUOfi9XeWxvZ3VqLmFzcHhkZAIGD2QWGAIDDzwrAAsBAA8WCB4IRGF0YUtleXMWAB4LXyFJdGVtQ291bnQCBx4JUGFnZUNvdW50AgEeFV8hRGF0YVNvdXJjZUl0ZW1Db3VudAIHZGQCBQ8PFgQfBWUfAmhkZAIJD2QWBAIBDw8WAh8FBR9HcnVweSBiZXogemFwbGFub3dhbnljaCB6YWrEmcSHZGQCAw88KwAOAgAUKwACZBcAARYCFgsPAgUUKwAFFCsABRYCHgpIZWFkZXJUZXh0BQlQcnplZG1pb3RkZGQFCVByemVkbWlvdBQrAAUWAh8RBQtQcm93YWR6xIVjeWRkZAUKUHJvd2FkemFjeRQrAAUWAh8RBQ1Gb3JtYSB6YWrEmcSHZGRkBQpGb3JtYVphamVjFCsABRYCHxEFDUxpY3piYSBsZWtjamlkZGQFDExpY3piYUdvZHppbhQrAAUWAh8RBQ1Gb3JtYSB6YWxpY3ouZGRkBQ9Gb3JtYVphbGljemVuaWFkZRQrAAALKXpUZWxlcmlrLldlYi5VSS5HcmlkQ2hpbGRMb2FkTW9kZSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNAE8KwAHAAspdVRlbGVyaWsuV2ViLlVJLkdyaWRFZGl0TW9kZSwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNAFkZGRkZmQCEQ8QZA8WA2YCAQICFgMFCER6aWVubmllBQpUeWdvZG5pb3dvBQxTZW1lc3RyYWxuaWUWAQIBZAIVDw8WBB4MU2VsZWN0ZWREYXRlBgAYpJczMdWIHhFfc2tpcE1NVmFsaWRhdGlvbmhkFgRmDxQrAAgPFgwfE2gfBQUTMjAxNy0xMS0yMi0wMC0wMC0wMB4RRW5hYmxlQXJpYVN1cHBvcnRoHgRTa2luBQdEZWZhdWx0HwRoHg1MYWJlbENzc0NsYXNzBQdyaUxhYmVsZBYGHgVXaWR0aBsAAAAAAABZQAcAAAAfCQURcmlUZXh0Qm94IHJpSG92ZXIfCgKCAhYGHxcbAAAAAAAAWUAHAAAAHwkFEXJpVGV4dEJveCByaUVycm9yHwoCggIWBh8XGwAAAAAAAFlABwAAAB8JBRNyaVRleHRCb3ggcmlGb2N1c2VkHwoCggIWBh8XGwAAAAAAAFlABwAAAB8JBRNyaVRleHRCb3ggcmlFbmFibGVkHwoCggIWBh8XGwAAAAAAAFlABwAAAB8JBRRyaVRleHRCb3ggcmlEaXNhYmxlZB8KAoICFgYfFxsAAAAAAABZQAcAAAAfCQURcmlUZXh0Qm94IHJpRW1wdHkfCgKCAhYGHxcbAAAAAAAAWUAHAAAAHwkFEHJpVGV4dEJveCByaVJlYWQfCgKCAmQCAg8UKwANDxYKBQ1TZWxlY3RlZERhdGVzDwWQAVRlbGVyaWsuV2ViLlVJLkNhbGVuZGFyLkNvbGxlY3Rpb25zLkRhdGVUaW1lQ29sbGVjdGlvbiwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAAFD1JlbmRlckludmlzaWJsZWcFEUVuYWJsZU11bHRpU2VsZWN0aAULU3BlY2lhbERheXMPBZMBVGVsZXJpay5XZWIuVUkuQ2FsZW5kYXIuQ29sbGVjdGlvbnMuQ2FsZW5kYXJEYXlDb2xsZWN0aW9uLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAAUSRmFzdE5hdmlnYXRpb25TdGVwAgwPFgYfFGgfFQUHRGVmYXVsdB8EaGRkFgQfCQULcmNNYWluVGFibGUfCgICFgQfCQUMcmNPdGhlck1vbnRoHwoCAmQWBB8JBQpyY1NlbGVjdGVkHwoCAmQWBB8JBQpyY0Rpc2FibGVkHwoCAhYEHwkFDHJjT3V0T2ZSYW5nZR8KAgIWBB8JBQlyY1dlZWtlbmQfCgICFgQfCQUHcmNIb3Zlch8KAgIWBB8JBTFSYWRDYWxlbmRhck1vbnRoVmlldyBSYWRDYWxlbmRhck1vbnRoVmlld19EZWZhdWx0HwoCAhYEHwkFCXJjVmlld1NlbB8KAgJkAhcPDxYEHxIGABiklzMx1YgfE2hkFgRmDxQrAAgPFgwfE2gfBQUTMjAxNy0xMS0yMi0wMC0wMC0wMB8UaB8VBQdEZWZhdWx0HwRoHxYFB3JpTGFiZWxkFgYfFxsAAAAAAABZQAcAAAAfCQURcmlUZXh0Qm94IHJpSG92ZXIfCgKCAhYGHxcbAAAAAAAAWUAHAAAAHwkFEXJpVGV4dEJveCByaUVycm9yHwoCggIWBh8XGwAAAAAAAFlABwAAAB8JBRNyaVRleHRCb3ggcmlGb2N1c2VkHwoCggIWBh8XGwAAAAAAAFlABwAAAB8JBRNyaVRleHRCb3ggcmlFbmFibGVkHwoCggIWBh8XGwAAAAAAAFlABwAAAB8JBRRyaVRleHRCb3ggcmlEaXNhYmxlZB8KAoICFgYfFxsAAAAAAABZQAcAAAAfCQURcmlUZXh0Qm94IHJpRW1wdHkfCgKCAhYGHxcbAAAAAAAAWUAHAAAAHwkFEHJpVGV4dEJveCByaVJlYWQfCgKCAmQCAg8UKwANDxYKBQ1TZWxlY3RlZERhdGVzDwWQAVRlbGVyaWsuV2ViLlVJLkNhbGVuZGFyLkNvbGxlY3Rpb25zLkRhdGVUaW1lQ29sbGVjdGlvbiwgVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMi4zLjEyMDUuMzUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49MTIxZmFlNzgxNjViYTNkNBQrAAAFD1JlbmRlckludmlzaWJsZWcFEUVuYWJsZU11bHRpU2VsZWN0aAULU3BlY2lhbERheXMPBZMBVGVsZXJpay5XZWIuVUkuQ2FsZW5kYXIuQ29sbGVjdGlvbnMuQ2FsZW5kYXJEYXlDb2xsZWN0aW9uLCBUZWxlcmlrLldlYi5VSSwgVmVyc2lvbj0yMDEyLjMuMTIwNS4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0FCsAAAUSRmFzdE5hdmlnYXRpb25TdGVwAgwPFgYfFGgfFQUHRGVmYXVsdB8EaGRkFgQfCQULcmNNYWluVGFibGUfCgICFgQfCQUMcmNPdGhlck1vbnRoHwoCAmQWBB8JBQpyY1NlbGVjdGVkHwoCAmQWBB8JBQpyY0Rpc2FibGVkHwoCAhYEHwkFDHJjT3V0T2ZSYW5nZR8KAgIWBB8JBQlyY1dlZWtlbmQfCgICFgQfCQUHcmNIb3Zlch8KAgIWBB8JBTFSYWRDYWxlbmRhck1vbnRoVmlldyBSYWRDYWxlbmRhck1vbnRoVmlld19EZWZhdWx0HwoCAhYEHwkFCXJjVmlld1NlbB8KAgJkAhkPDxYCHwUFB0ZpbHRydWpkZAIbDxAPFgQfBQUKUGxhbiBzZXNqaR8CaGRkZGQCHQ8WBh4FdmFsdWUFCFd5ZHJ1a3VqHgVjbGFzcwUJcHJ6eWNpc2tNHgdvbmNsaWNrBWd3aW5kb3cub3BlbignUG9kekdvZHpEcnVrLmFzcHg%2FZnJvbT0yMDE3MTEyMCZ0bz0yMDE3MTEyNiZzZXNqYT0wJnNpZz1NUE43JTJmMEtPS0JVQ21BQmhPSGxhTXpVZ0JwayUzZCcpZAIfDxYGHxgFG1d5ZHJ1a3VqIHBvIGRuaWFjaCB0eWdvZG5pYR8ZBQlwcnp5Y2lza00fAmhkAiEPFgYfGAUhUG9iaWVyeiBwbGFuIHcgZm9ybWFjaWUgaUNhbGVuZGFyHxkFCXByenljaXNrTR8aBXF3aW5kb3cub3BlbignUG9kekdvZHpEcnVrLmFzcHg%2FdHlwPWljcyZmcm9tPTIwMTcxMTIwJnRvPTIwMTcxMTI2JnNlc2phPTAmc2lnPUV5R0wlMmJ1VXNLeTR3MWolMmZ0akpQb0Z6bHEwRlElM2QnKWQCIw8WBh8YBTBQb2JpZXJ6IHBsYW4gdyBmb3JtYWNpZSBpQ2FsZW5kYXIgZGxhIEdyb3VwIFdpc2UfGQUJcHJ6eWNpc2tNHwJoZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WCQU2Y3RsMDAkY3RsMDAkVG9wTWVudVBsYWNlSG9sZGVyJHd1bWFzdGVyTWVudVRvcCRtZW51VG9wBTBjdGwwMCRjdGwwMCRUb3BNZW51UGxhY2VIb2xkZXIkTWVudVRvcDIkbWVudVRvcDIFN2N0bDAwJGN0bDAwJENvbnRlbnRQbGFjZUhvbGRlciR3dW1hc3Rlck1lbnVMZWZ0JHJhZE1lbnUFQGN0bDAwJGN0bDAwJENvbnRlbnRQbGFjZUhvbGRlciRSaWdodENvbnRlbnRQbGFjZUhvbGRlciRyYWREYXRhT2QFSWN0bDAwJGN0bDAwJENvbnRlbnRQbGFjZUhvbGRlciRSaWdodENvbnRlbnRQbGFjZUhvbGRlciRyYWREYXRhT2QkY2FsZW5kYXIFSWN0bDAwJGN0bDAwJENvbnRlbnRQbGFjZUhvbGRlciRSaWdodENvbnRlbnRQbGFjZUhvbGRlciRyYWREYXRhT2QkY2FsZW5kYXIFQGN0bDAwJGN0bDAwJENvbnRlbnRQbGFjZUhvbGRlciRSaWdodENvbnRlbnRQbGFjZUhvbGRlciRyYWREYXRhRG8FSWN0bDAwJGN0bDAwJENvbnRlbnRQbGFjZUhvbGRlciRSaWdodENvbnRlbnRQbGFjZUhvbGRlciRyYWREYXRhRG8kY2FsZW5kYXIFSWN0bDAwJGN0bDAwJENvbnRlbnRQbGFjZUhvbGRlciRSaWdodENvbnRlbnRQbGFjZUhvbGRlciRyYWREYXRhRG8kY2FsZW5kYXIduretGx73XhKsiKFtF%2Fi7qTO9Aw%3D%3D&__VIEWSTATEGENERATOR=C842751A&__EVENTVALIDATION=%2FwEWEQLZ39abDwKBq8OvBwKjovauBAKn6MnOBgLYtMHMCgLnprz5BQL%2F%2B4isBALSn6sTAoud7qYFApy0vaUKAoWx04wMAq2z3VAC1bGiiAgCsP%2BiwQsCnPe4ywQClsvz2gcC%2BJ65Pe18lkd%2F6b11SYoSZHmdY5E%2BtWel&ctl00_ctl00_TopMenuPlaceHolder_wumasterMenuTop_menuTop_ClientState=&ctl00_ctl00_ContentPlaceHolder_wumasterMenuLeft_radMenu_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24rbJak=Semestralnie&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataOd=2017-11-22&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataOd%24dateInput=22.11.2017&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_dateInput_ClientState=&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_calendar_SD=%5B%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_calendar_AD=%5B%5B1980%2C1%2C1%5D%2C%5B2099%2C12%2C30%5D%2C%5B2017%2C11%2C22%5D%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataOd_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataDo=2017-11-22&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24radDataDo%24dateInput=22.11.2017&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_dateInput_ClientState=&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_calendar_SD=%5B%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_calendar_AD=%5B%5B1980%2C1%2C1%5D%2C%5B2099%2C12%2C30%5D%2C%5B2017%2C11%2C22%5D%5D&ctl00_ctl00_ContentPlaceHolder_RightContentPlaceHolder_radDataDo_ClientState=&ctl00%24ctl00%24ContentPlaceHolder%24RightContentPlaceHolder%24hid_Temp=", 
                    Encoding.UTF8, 
                    "application/x-www-form-urlencoded");
                siteDocumentWithSchedule = await _communicationService.SendAsync(IoC.Settings.scheduleURL, modeToSemesterViewContent);

                // Log out
                await _communicationService.SendAsync(IoC.Settings.logOutURL);
            }
            catch (HttpRequestException ex)
            {
                Logger.Log("Error while switching to schedule page", Logger.LogLevel.Error);
                throw;
            }

            return GenerateDaysFromSiteDocument(siteDocumentWithSchedule);
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
                    Status = recordData.getValueAt(ClassRecordData.Status),
                });

            }

            return result;
        }
    }
}
