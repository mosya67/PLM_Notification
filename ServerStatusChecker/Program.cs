
using Microsoft.AspNetCore.Http.HttpResults;

#pragma warning disable CS4014

namespace ServerStatusChecker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();
            builder.Services.AddHostedService<HealthCheckWorker>();
            // зарегать бота
            // зарегать бд
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
