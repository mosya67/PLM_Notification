using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ServerStatusChecker
{
    public class TelegramBot
    {
        private readonly ITelegramBotClient client;

        public TelegramBot(ITelegramBotClient botClient)
        {
            client = botClient;
            client.StartReceiving(Update, Error);
        }

        public async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type == UpdateType.Message)
            {
                await HandleMessage(client, update.Message);
            }
            // обработчик кнопок в боте(кнопок, которок под сообщением висят). Оставлю на всякий, на будущее. Нужно просто реализовать метод, что напсиан ниже
            else if (update?.Type == UpdateType.CallbackQuery) 
            {
                //await HandleCallBackQuery(client, update.CallbackQuery);
            }
        }

        public Task Error<T>(ITelegramBotClient client, T ex, CancellationToken token)
            where T : Exception
        {
            string message = '\n' + ex.GetType().Name + '\n' + ex.Message + '\n' + ex.InnerException?.Message + '\n' + ex.StackTrace;
            Console.WriteLine(message);
            // TODO: отправить в тг
            throw new Exception(message);
        }

        public async Task HandleMessage(ITelegramBotClient client, Message message)
        {
            if (message?.Text?.ToLower() == "/status")
            {
                // Вызов метода проверки сервера
                bool result = await HttpHelper.CheckStatusAsync($"http://union-test/Health");
                if (!result) await client.SendMessage(message.Chat.Id, "Сервак упал");
            }
        }
    }
}
