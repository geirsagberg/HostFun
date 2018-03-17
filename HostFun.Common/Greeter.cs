using System;

namespace HostFun.Common
{
    public class Greeter : IGreeter
    {
        public void Greet()
        {
            Console.WriteLine($"Hello world! The time is {DateTimeOffset.Now:T}");
        }
    }
}