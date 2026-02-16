
using Microsoft.AspNetCore.Http.HttpResults;

#pragma warning disable CS4014

namespace ServerStatusChecker
{
    public class Program
    {
        const int FiveMinInMillisec = 300_000;
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddHostedService<HealthCheckWorker>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
