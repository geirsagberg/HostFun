using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using HostFun.Common;
using Microsoft.EntityFrameworkCore;
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
            var host = new HostBuilder()
                .ConfigureAppConfiguration(config => {
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((context, services) => {
                    //services.AddSingleton<IHostedService, OtherService>();
                    //services.AddSingleton<IHostedService, NotificationService>();
                    services.AddHangfire(config => {
                        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                        config.UseSqlServerStorage(connectionString, new SqlServerStorageOptions {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true
                        });
                    });
                    services.AddSingleton<IGreeter>(new Greeter());
                    services.AddSingleton<IHostedService, HangfireService>();
                })
                .ConfigureLogging(logging => {
                    logging.AddConsole();
                })
                .UseConsoleLifetime()
                .Build()
                ;

            {
                var connectionString = host.Services.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
                var dbOptions = new DbContextOptionsBuilder()
                    .UseSqlServer(connectionString)
                    .Options;
                using (var dbContext = new DbContext(dbOptions)) {
                    await dbContext.Database.EnsureCreatedAsync();
                }
            }

            await host.RunAsync();
        }
    }
}
