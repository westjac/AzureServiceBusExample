using Microsoft.EntityFrameworkCore;
using ServiceBus.Receiver.data.Entities;

namespace ServiceBus.Receiver.data
{
    public class ServiceBusDbContext : DbContext
    {
        public ServiceBusDbContext(DbContextOptions<ServiceBusDbContext> options)
        {
                
        }
        public DbSet<Error> Errors { get; set; }
    }
}
