using System.Threading.Tasks;
using Hangfire;
using HostFun.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostFun
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration(config => {
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((context, services) => {
                    //services.AddSingleton<IHostedService, OtherService>();
                    //services.AddSingleton<IHostedService, NotificationService>();
                    services.AddHangfire(config => {
                        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                        config.UseSqlServerStorage(connectionString);
                    });
                    services.AddSingleton<IGreeter>(new Greeter());
                    services.AddSingleton<IHostedService, HangfireService>();
                })
                .ConfigureLogging(logging => {
                    logging.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}