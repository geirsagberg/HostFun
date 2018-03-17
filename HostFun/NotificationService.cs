using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HostFun
{
    internal class NotificationService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true) {
                Console.WriteLine($"The current time is: {DateTimeOffset.Now:T}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}