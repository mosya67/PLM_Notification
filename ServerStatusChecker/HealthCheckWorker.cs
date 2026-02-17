namespace ServerStatusChecker
{
    public class HealthCheckWorker : BackgroundService
    {
        private readonly IConfiguration config;

        public HealthCheckWorker(IConfiguration config)
        {
            this.config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await HttpHelper.CheckStatusAsync($"http://union-test/Health");

                    if (!response)
                        Console.WriteLine($"сервер упаль");
                    else
                        Console.WriteLine("Живой");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // Ждем 6 секунд перед следующей итерацией
                await Task.Delay(TimeSpan.FromSeconds(config.GetValue<long>("DelayInSeconds")), stoppingToken);
            }
        }
    }
}
