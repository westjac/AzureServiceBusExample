using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ServiceBus.MessageEntities;

//DI, Serilog, Settings

namespace ServiceBus.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildSerilogConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //services.AddTransient<IGreetingService, GreetingService>();
                })
                .UseSerilog()
                .Build();

            //var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
            ReceiveAndProcessErrorMessages(1);

        }

        static void BuildSerilogConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        private static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = Encoding.UTF8.GetString(args.Message.Body);
            var errorMessage = JsonSerializer.Deserialize<ErrorMessage>(body);

            //Process the message
            ErrorMessageLogic(errorMessage);

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        private static void ErrorMessageLogic(ErrorMessage errorMessage)
        {
            Console.WriteLine($"Error Id {errorMessage.Id}: {errorMessage.Type} and occurred {errorMessage.Time}");
        }

        // handle any service bus errors when receiving messages
        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private static async void ReceiveAndProcessErrorMessages(int threads)
        {
            var client = new ServiceBusClient(Settings.ConnectionString);
            var options = new ServiceBusProcessorOptions()
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = threads,
                MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(10)
            };
            var processor = client.CreateProcessor(Settings.QueueName, options);

            try
            {
                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                //Start processing
                await processor.StartProcessingAsync();

                Console.WriteLine("Press any key to stop processing...");
                Console.ReadKey();
                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");

            }
            finally
            {
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }

        }

    }
}
