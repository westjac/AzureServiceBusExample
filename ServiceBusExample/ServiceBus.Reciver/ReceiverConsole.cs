using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using ServiceBus.Entities;

namespace ServiceBus.Receiver
{
    internal class ReceiverConsole
    {
        private readonly WebServiceRequestHandler _webServiceRequestHandler;
        public ReceiverConsole(WebServiceRequestHandler webServiceRequestHandler)
        {
            _webServiceRequestHandler = webServiceRequestHandler;
        }

        public void Run()
        {
            ReceiveAndProcessErrorMessages(1);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = Encoding.UTF8.GetString(args.Message.Body);
            var message = JsonSerializer.Deserialize<WebServiceRequest>(body);

            //Process the message
            _webServiceRequestHandler.WriteToConsole(message);
            _webServiceRequestHandler.WriteToDatabase(message);

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any service bus errors when receiving messages
        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async void ReceiveAndProcessErrorMessages(int threads)
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