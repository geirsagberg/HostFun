using Hangfire;
using Hangfire.SqlServer;
using HostFun.Common;

namespace HostFun.SecondApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var storage = new SqlServerStorage(
                "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=HostFun;MultipleActiveResultSets=True");
            var client = new BackgroundJobClient(storage);

            client.Enqueue<IGreeter>(greeter => greeter.Greet());
        }
    }
}