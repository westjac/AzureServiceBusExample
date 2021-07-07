using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceBus.Config;
using Azure.Messaging.ServiceBus;
using static System.Console;
using Error = ServiceBus.Sender.Entities.Error;

namespace ServiceBus.Sender
{
    class SenderConsole
    {
        private static ServiceBusClient _client;
        private static ServiceBusSender _sender;
        private static Random _random;
        static async Task Main(string[] args)
        {
            _random = new Random();

            WriteLine("Sender Console - Enter number of messages to send (0 to exit): ");
            int messageCount = Convert.ToInt32(ReadLine());

            while (messageCount != 0)
            {
                for (int i = 0; i < messageCount; i++)
                {
                    await SendError();
                    //await SendErrorNoBody();
                    WriteLine($"Sent {i+1} messages so far.", ConsoleColor.Magenta);
                }
                WriteLine("Sender Console - Enter number of messages to send (0 to exit): ");
                messageCount = Convert.ToInt32(ReadLine());
            }

            WriteLine("Sender Console - Complete", ConsoleColor.Green);
        }

        static async Task SendError()
        {
            _client = new ServiceBusClient(Settings.ConnectionString);
            _sender = _client.CreateSender(Settings.QueueName);
            WriteLine("Sending Error...", ConsoleColor.DarkGray);
            
            var error = new Error()
            {
                Host = "Error Console",
                Id = _random.Next(),
                Type = "System.Business.NotFoundExample",
                StatusCode = 0,
                Message = $"Could not find an Example ExampleId: {_random.Next()}",
                Time = DateTime.Now,
            };

            // Serialize the errorMessage object to JSON
            var jsonErrorMessage = JsonSerializer.Serialize(error);

            // Create a Message for the Service Bus
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonErrorMessage))
            {
                ContentType = "application/json",
                Subject = "ErrorMessage"
            };

            try
            {
                await _sender.SendMessageAsync(message);
            }
            finally
            {
                await _sender.DisposeAsync();
                await _client.DisposeAsync();
            }

            WriteLine("Done!", ConsoleColor.DarkGray);
        }

        static async Task SendErrorNoBody()
        {
            WriteLine("SendErrorNoBody");

            var message = new ServiceBusMessage()
            {
                Subject = "ErrorMessage No Body"
            };

            message.ApplicationProperties.Add("SystemId", 1367);
            message.ApplicationProperties.Add("ErrorType", "System.NotFound");
            message.ApplicationProperties.Add("ErrorTime", DateTime.Now);

            _client = new ServiceBusClient(Settings.ConnectionString);
            _sender = _client.CreateSender(Settings.QueueName);

            Write("Sending error message with no body...");
            try
            {
                await _sender.SendMessageAsync(message);
            }
            finally
            {
                await _sender.DisposeAsync();
                await _client.DisposeAsync();
            }

            WriteLine("Done!");
        }

        private static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = tempColor;
        }
    }
}
