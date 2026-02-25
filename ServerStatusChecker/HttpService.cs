using System.Net;

namespace ServerStatusChecker
{
    public static class HttpService
    {
        public static async Task<bool> CheckStatusAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
    }
}
