using Telegram.Bot.Types;

namespace ServerStatusChecker
{
    public interface INotificationService
    {
        public Task NotifyAsync(long user, string text);
        public Task NotifyAsync(IEnumerable<long> users, string text);
    }
}