using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Receiver.Data;

//DI, Serilog, Settings

namespace ServiceBus.Receiver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Setup DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<ReceiverConsole>()
                .AddSingleton<WebServiceRequestHandler>()
                .AddDbContext<ServiceBusDbContext>()
                .BuildServiceProvider();

            serviceProvider.GetService<ReceiverConsole>()
                .Run();
            

        }
    }
}
