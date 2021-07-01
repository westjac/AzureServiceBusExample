using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ServiceBus.Config;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp.Framing;
using ServiceBus.MessageEntities;
using static System.Console;

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

            WriteLine("Sender Console - Hit Enter");
            ReadLine();

            await SendError();

            WriteLine("Sender Console - Complete");
            ReadLine();
        }

        static async Task SendError()
        {
            _client = new ServiceBusClient(Settings.ConnectionString);
            _sender = _client.CreateSender(Settings.QueueName);
            WriteLine("Sending Error...");
            
            var error = new ErrorMessage()
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
                WriteLine("An error message has been published to the queue.");
            }
            finally
            {
                await _sender.DisposeAsync();
                await _client.DisposeAsync();
            }

            WriteLine("Done!");
        }
    }
}
