using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Receiver.data;

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
                .AddDbContext<ServiceBusDbContext>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=ServiceBusExampleDatabase;Integrated Security=True;"))
                .BuildServiceProvider();

            var console = serviceProvider.GetService<ReceiverConsole>();
            console.Run();

        }
    }
}
