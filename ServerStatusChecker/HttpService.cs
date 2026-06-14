using System.Net;

namespace ServerStatusChecker
{
    public static class HttpService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<bool> CheckStatusAsync(string url)
        {
            var response = await httpClient.GetAsync(url);
            return response.StatusCode == HttpStatusCode.OK ? true : false;
        }
    }
}
