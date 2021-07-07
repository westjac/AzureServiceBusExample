using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBus.Receiver.Data;
using ServiceBus.Receiver.Data.Entities;

namespace ServiceBus.Receiver
{
    class ErrorHandler
    {
        private ServiceBusDbContext _context;
        public ErrorHandler(ServiceBusDbContext context)
        {
            _context = context;
        }

        public void WriteToConsole(Error error)
        {
            Console.WriteLine($"Error Id {error.Id}: {error.Type} and occurred {error.Time}");
        }

        public void WriteToDatabase(Error error)
        {
            _context.Add(error);
            _context.SaveChanges();
        }
    }
}
