using Telegram.Bot;
using TelegramTypes = Telegram.Bot.Types;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ServerStatusChecker
{
    public class TelegramBot : BackgroundService
    {
        private readonly ITelegramBotClient client;
        private readonly IConfiguration config;
        private readonly INotificationService notificationService;

        public TelegramBot(ITelegramBotClient client, IConfiguration config, INotificationService notificationService)
        {
            this.client = client;
            this.config = config;
            this.notificationService = notificationService;
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
            if (message?.Text == "/status")
            {
                // Вызов метода проверки сервера
                bool result = await HttpHelper.CheckStatusAsync(config.GetValue<string>("EndPointPLM"));
                if (result)
                    await notificationService.NotifyAsync(message.Chat.Id, "Сервак жив");
                else
                    await notificationService.NotifyAsync(message.Chat.Id, "Сервак упал");
            }
            else if (message?.Text == "/registration")
            {
                await notificationService.NotifyAsync(message.Chat.Id, "Пока нет функционала для этой команды");
                // Добавление ChatId в бд
            }
            else
            {
                await notificationService.NotifyAsync(message.Chat.Id, "Неизвестаная команда");
            }
        }
    }
}
