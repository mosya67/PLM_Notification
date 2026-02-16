using System.Net;

namespace ServerStatusChecker
{
    public static class HttpHelper
    {
        public static async Task<bool> CheckStatusAsync(string url)
        {
            // Здесь вы можете отправить запрос к вашему ASP.NET Core API
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
        }
    }
}
