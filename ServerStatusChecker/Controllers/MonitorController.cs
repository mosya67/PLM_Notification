using Microsoft.AspNetCore.Mvc;

namespace ServerStatusChecker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public MonitorController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet($"/Health")]
        public async Task<IActionResult> CheckServer(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                bool isUp = response.IsSuccessStatusCode; // true, если код состояния 2xx
                return Ok(new { Status = isUp ? "UP" : "DOWN" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при опросе сервера: {ex.Message}");
                return Ok(new { Status = "DOWN" }); // сервер недоступен или произошла ошибка
            }
        }
    }
}
