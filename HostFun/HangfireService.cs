using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Hosting;

namespace HostFun
{
    public class HangfireService : IHostedService
    {
        private readonly IEnumerable<IBackgroundProcess> additionalProcesses;
        private readonly BackgroundJobServerOptions options;
        private readonly JobStorage storage;
        private BackgroundJobServer server;

        public HangfireService(JobStorage storage, IEnumerable<BackgroundJobServerOptions> options,
            IEnumerable<IBackgroundProcess> additionalProcesses)
        {
            this.storage = storage;
            this.options = options.FirstOrDefault() ?? new BackgroundJobServerOptions();
            this.additionalProcesses = additionalProcesses;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            server = new BackgroundJobServer(options, storage, additionalProcesses);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            server.Dispose();
            return Task.CompletedTask;
        }
    }
}