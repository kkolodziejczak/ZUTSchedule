using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.core
{
    /// <summary>
    /// Core communication service
    /// </summary>
    public class CommunicationService
    {
        private static HttpClientHandler _handle;
        private static HttpClient _client;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommunicationService()
        {
            _handle = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                
            };

            _client = new HttpClient(_handle);
        }

        /// <summary>
        /// Cleanup afterwards
        /// </summary>
        ~CommunicationService()
        {
            _client.Dispose();
            _handle.Dispose();
        }

        /// <exception cref="HttpRequestException"/>
        public async Task<string> SendAsync(string url, HttpContent httpContent)
        {
            try
            {
                var httpResponse = await _client.PostAsync(url, httpContent);
                Logger.Log($"POST: {url}", Logger.LogLevel.Info);
                return await httpResponse.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Logger.Log($"Something went bad with POST request {url} \n {ex.Message} \n\n {ex.StackTrace}", Logger.LogLevel.Error);
                throw;
            }
        }

        /// <exception cref="HttpRequestException"/>
        public async Task<string> SendAsync(string url)
        {
            try
            {
                var httpResponse = await _client.GetAsync(url);
                Logger.Log($"GET: {url}", Logger.LogLevel.Info);
                return await httpResponse.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Logger.Log($"Something went bad with GET request {url} \n {ex.Message} \n\n {ex.StackTrace}", Logger.LogLevel.Error);
                throw;
            }
        }

    }
}
