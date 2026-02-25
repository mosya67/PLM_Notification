using Core;
using Database.Model;

namespace ServerStatusChecker.BackgroundServices
{
    /// <summary>
    /// Сервис для проверки состояния сервера. Работает в фоне
    /// </summary>
    public class StatusChecker : BackgroundService
    {
        private readonly IConfiguration config;
        private readonly INotificationService notificationService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StatusChecker(IConfiguration config, INotificationService notificationService, IServiceScopeFactory serviceScopeFactory)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await HttpService.CheckStatusAsync(config.GetValue<string>("EndPointPLM"));
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        if (!response)
                        {
                            var getAllUsers = scope.ServiceProvider.GetRequiredService<IReadCommand<IEnumerable<User>>>();
                            IEnumerable<User> users = await getAllUsers.Read();
                            await notificationService.NotifyAsync(users.Select(e => e.UserId), "Сервер не отвечает!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(config.GetValue<long>("DelayInSeconds")), stoppingToken);
            }
        }
    }
}
