
using Core;
using Database;
using Database.Model;
using Database.ReadCommands;
using Database.WriteCommands;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ServerStatusChecker.BackgroundServices;
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
            builder.Services.AddSingleton<string>(builder.Configuration.GetValue<string>("DefaultConnectionString"));

            builder.Services.AddHostedService<HealthCheckWorker>();
            builder.Services.AddHostedService<TelegramBot>();
            builder.Services.AddScoped<Context>();
            builder.Services.AddKeyedScoped<IWriteCommand<long>, AddUserWithChecks>("AddUser");
            builder.Services.AddKeyedScoped<IWriteCommand<long>, DeleteUser>("DeleteUser");
            builder.Services.AddScoped<IReadCommand<IEnumerable<TgUser>>, GetAllsers>();
            builder.Services.AddScoped<IReadCommand<bool, long>, CheckUserExists>();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.Run();
        }
    }
}
