using Microsoft.EntityFrameworkCore;
using ServiceBus.Entities;

namespace ServiceBus.Receiver.Data
{
    public class ServiceBusDbContext : DbContext
    {
        public DbSet<WebServiceRequest> WebServiceRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source = localhost; Initial Catalog = ServiceBusExampleDatabase; Integrated Security = True; ");
        }
    }
}
