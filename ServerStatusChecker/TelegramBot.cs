using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ServerStatusChecker
{
    public class TelegramBot
    {
        private readonly ITelegramBotClient client;
        const int FiveMinInMillisec = 300_000;

        public TelegramBot(ITelegramBotClient botClient)
        {
            client = botClient;
            client.StartReceiving(Update, Error);
        }

        async public Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type == UpdateType.Message)
            {
                await HandleMessage(client, update.Message);
            }
            else if (update?.Type == UpdateType.CallbackQuery)
            {
                //await HandleCallBackQuery(client, update.CallbackQuery);
            }
        }

        private Task Error<T>(ITelegramBotClient client, T ex, CancellationToken token)
            where T : Exception
        {
            string message = '\n' + ex.GetType().Name + '\n' + ex.Message + '\n' + ex.InnerException?.Message + '\n' + ex.StackTrace;
            Console.WriteLine(message);
            // TODO: отправить в тг
            throw new Exception(message);
        }

        async Task HandleMessage(ITelegramBotClient client, Message message)
        {
            if (message?.Text?.ToLower() == "/status")
            {
                // Вызов метода проверки сервера
                bool result = await CheckServerStatusAsync("http://example.com");
                if (!result) await client.SendMessage(message.Chat.Id, "Сервак упал");
            }

            if (message?.Text?.ToLower() == "/startChekingStatus")
            {
                await Task.Run(async () =>
                {
                    while(true)
                    {
                        // Вызов метода проверки сервера
                        bool result = await CheckServerStatusAsync("http://example.com");
                        if (!result) await client.SendMessage(message.Chat.Id, "Сервак упал");

                        // 300.000 миллисеккунд это 5 минут
                        Thread.Sleep(FiveMinInMillisec);
                    }
                });
            }
        }

        private async Task<bool> CheckServerStatusAsync(string url)
        {
            // Здесь вы можете отправить запрос к вашему ASP.NET Core API
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"http://localhost:5000/status/{url}");
                return response.IsSuccessStatusCode ? true : false;
            }
        }
    }
}
