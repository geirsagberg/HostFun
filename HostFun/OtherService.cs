using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HostFun
{
    internal class OtherService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true) {
                Console.WriteLine($"The current milliseconds is: {DateTimeOffset.Now.Millisecond}");
                await Task.Delay(200, stoppingToken);
            }
        }
    }
}