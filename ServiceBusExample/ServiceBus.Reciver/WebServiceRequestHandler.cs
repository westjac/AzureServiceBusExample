using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBus.Receiver.Data;
using ServiceBus.Entities;

namespace ServiceBus.Receiver
{
    class WebServiceRequestHandler
    {
        private ServiceBusDbContext _context;
        public WebServiceRequestHandler(ServiceBusDbContext context)
        {
            _context = context;
        }

        public void WriteToConsole(WebServiceRequest webServiceRequest)
        {
            Console.WriteLine($"Web Service Request Id {webServiceRequest.Id}: {webServiceRequest.Type} and occurred {webServiceRequest.Time}");
        }

        public void WriteToDatabase(WebServiceRequest webServiceRequest)
        {
            _context.Add(webServiceRequest);
            _context.SaveChanges();
        }
    }
}
