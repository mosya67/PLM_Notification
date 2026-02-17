using Telegram.Bot;
using Telegram.Bot.Types;

namespace ServerStatusChecker
{
    public class TelegramNotification : INotificationService
    {
        private readonly ITelegramBotClient client;

        public TelegramNotification(ITelegramBotClient client)
        {
            this.client = client;
        }
        public async Task NotifyAsync(long chatId, string text)
        {
            await this.client.SendMessage(chatId, text);
        }

        public async Task NotifyAsync(IEnumerable<long> users, string text)
        {
            var tasks = users.Select(user => NotifyAsync(user, text));
            await Task.WhenAll(tasks);
        }
    }
}
