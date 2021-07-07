using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServiceBus.Receiver.data.Entities
{
    public class Error
    {
        private static ServiceBusDbContext _context;

        public Error() {}
        public Error(ServiceBusDbContext context)
        {
            _context = context;
        }

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
