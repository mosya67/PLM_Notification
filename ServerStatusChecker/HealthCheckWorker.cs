namespace ServerStatusChecker
{
    public class HealthCheckWorker : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HealthCheckWorker(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await HttpHelper.CheckStatusAsync($"http://union-test/Health");

                    if (response)
                        Console.WriteLine("сервер воркинг");
                    else
                        Console.WriteLine($"сервер упаль");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // Ждем 6 секунд перед следующей итерацией
                await Task.Delay(TimeSpan.FromSeconds(6), stoppingToken);
            }
        }
    }
}
