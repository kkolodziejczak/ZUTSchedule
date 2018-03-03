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

        public string LastResponse { get; private set; }

        /// <summary>
        /// Returns last response
        /// </summary>
        /// <returns></returns>
        public string GetLastResponse() => LastResponse;

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
                Logger.Info($"POST: {url}");
                LastResponse = await httpResponse.Content.ReadAsStringAsync();
                return LastResponse;
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"Something went bad with POST request {url} \n {ex.Message} \n\n {ex.StackTrace}");
                throw;
            }
        }

        /// <exception cref="HttpRequestException"/>
        public async Task<string> SendAsync(string url)
        {
            try
            {
                var httpResponse = await _client.GetAsync(url);
                Logger.Info($"GET: {url}");
                return await httpResponse.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"Something went bad with GET request {url} \n {ex.Message} \n\n {ex.StackTrace}");
                throw;
            }
        }

    }
}
