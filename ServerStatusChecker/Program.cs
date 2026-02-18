using Core;
using Database;
using Database.Model;
using Database.ReadCommands;
using Database.WriteCommands;
using ServerStatusChecker.BackgroundServices;
using Telegram.Bot;

#pragma warning disable CS4014

namespace ServerStatusChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration.GetValue<string>("BotToken")));
            builder.Services.AddSingleton<INotificationService, TelegramNotification>();
            builder.Services.AddSingleton<string>(builder.Configuration.GetValue<string>("DefaultConnectionString"));

            builder.Services.AddHostedService<StatusChecker>();
            builder.Services.AddHostedService<TelegramBot>();
            builder.Services.AddScoped<Context>();
            AddDbMethods(builder);
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.Run();
        }

        static void AddDbMethods(WebApplicationBuilder builder)
        {
            builder.Services.AddKeyedScoped<IWriteCommand<long>, AddUserWithChecks>("AddUser");
            builder.Services.AddKeyedScoped<IWriteCommand<long>, DeleteUser>("DeleteUser");
            builder.Services.AddScoped<IReadCommand<IEnumerable<User>>, GetAllsers>();
            builder.Services.AddScoped<IReadCommand<bool, long>, CheckUserExists>();
        }
    }
}
