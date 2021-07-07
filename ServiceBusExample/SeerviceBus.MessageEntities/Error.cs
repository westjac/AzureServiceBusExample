using System;

namespace ServiceBus.MessageEntities
{
    public class Error
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DateTime Time { get; set; }

        public void WriteToConsole()
        {
            Console.WriteLine($"Error Id {Id}: {Type} and occurred {Time}");
        }

        public void WriteToDatabase()
        {
            //TODO: add EF logic
        }
    }
}
