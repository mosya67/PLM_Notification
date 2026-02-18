using Core;
using Database.Model;
using Database.ReadCommands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramTypes = Telegram.Bot.Types;

namespace ServerStatusChecker.BackgroundServices
{
    public class TelegramBot : BackgroundService
    {
        private readonly ITelegramBotClient client;
        private readonly IConfiguration config;
        private readonly INotificationService notificationService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TelegramBot(ITelegramBotClient client, IConfiguration config, INotificationService notificationService, IServiceScopeFactory serviceScopeFactory)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.client.StartReceiving(Update, Error, cancellationToken: stoppingToken);
        }

        private async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update?.Type == UpdateType.Message)
            {
                await HandleMessage(client, update.Message);
            }
        }

        private async Task Error<T>(ITelegramBotClient client, T ex, CancellationToken token)
            where T : Exception
        {
            string message = '\n' + ex.GetType().Name + '\n' + ex.Message + '\n' + ex.InnerException?.Message + '\n' + ex.StackTrace;
            Console.WriteLine(message);
            throw new Exception(message);
        }

        private async Task HandleMessage(ITelegramBotClient client, Message message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var checkUserExists = scope.ServiceProvider.GetRequiredService<IReadCommand<bool, long>>();
                var addUser = scope.ServiceProvider.GetKeyedService<IWriteCommand<long>>("AddUser");
                var deleteUser = scope.ServiceProvider.GetKeyedService<IWriteCommand<long>>("DeleteUser");
                if (message?.Text == "/status")
                {
                    // Вызов метода проверки сервера
                    bool result = await HttpService.CheckStatusAsync(config.GetValue<string>("EndPointPLM"));
                    if (result)
                        await notificationService.NotifyAsync(message.Chat.Id, "Сервак живой");
                    else
                        await notificationService.NotifyAsync(message.Chat.Id, "Сервак не отвечает!");
                }
                else if (message?.Text == "/subscribe")
                {
                    if (await checkUserExists.Read(message.Chat.Id))
                    {
                        await notificationService.NotifyAsync(message.Chat.Id, "Вы уже подписаны");
                        return;
                    }

                    await addUser.Write(message.Chat.Id);
                    await notificationService.NotifyAsync(message.Chat.Id, "Вы подписались на уведомления о сбоях сервера");
                }
                else if (message?.Text == "/unsubscribe")
                {
                    if (!await checkUserExists.Read(message.Chat.Id))
                    {
                        await notificationService.NotifyAsync(message.Chat.Id, "Вы и так не подписаны");
                        return;
                    }

                    await deleteUser.Write(message.Chat.Id);
                    await notificationService.NotifyAsync(message.Chat.Id, "Вы отписались от уведомлений о сбоях сервера");
                }
                else
                {
                    await notificationService.NotifyAsync(message.Chat.Id, "Неизвестаная команда");
                }
            }
        }
    }
}
