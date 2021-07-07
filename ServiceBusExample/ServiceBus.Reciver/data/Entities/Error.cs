using System;

namespace ServiceBus.Receiver.Data.Entities
{
    public class Error
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DateTime Time { get; set; }
    }
}
