using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Sender.Entities
{
    internal class Error
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DateTime Time { get; set; }
    }
}
