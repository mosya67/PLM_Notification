
using Microsoft.AspNetCore.Http.HttpResults;
using Telegram.Bot;
using Telegram.Bot.Types;

#pragma warning disable CS4014

namespace ServerStatusChecker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration.GetValue<string>("BotToken")));
            builder.Services.AddSingleton<INotificationService, TelegramNotification>();
            builder.Services.AddHostedService<HealthCheckWorker>();
            builder.Services.AddHostedService<TelegramBot>();
            // зарегать бд
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.Run();
        }
    }
}
