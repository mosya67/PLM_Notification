using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ServerStatusChecker
{
    public class TelegramBot
    {
        private readonly ITelegramBotClient client;
        private readonly IConfiguration config;

        public TelegramBot(ITelegramBotClient client, IConfiguration config)
        {
            this.client = client;
            this.config = config;
        }

        public async Task StartBot()
        {
            this.client.StartReceiving(Update, Error);
        }

        private async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type == UpdateType.Message)
            {
                await HandleMessage(client, update.Message);
            }
            // обработчик кнопок в боте(кнопок, которок под сообщением висят). Оставлю на всякий, на будущее
            else if (update?.Type == UpdateType.CallbackQuery) 
            {
                //await HandleCallBackQuery(client, update.CallbackQuery);
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
            if (message?.Text?.ToLower() == "/status")
            {
                // Вызов метода проверки сервера
                bool result = await HttpHelper.CheckStatusAsync($"http://union-test/Health");
                if (!result) await SendMessageAsync(message.Chat.Id, "Сервак упал");
            }
            if (message?.Text?.ToLower() == "/registration")
            {
                // Добавление ChatId в бд
            }
        }

        public async Task<Message> SendMessageAsync(ChatId chatId, string text)
        {
            return await this.client.SendMessage(chatId, text);
        }
    }
}
