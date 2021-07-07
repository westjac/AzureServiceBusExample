using Microsoft.EntityFrameworkCore;
using ServiceBus.Receiver.Data.Entities;

namespace ServiceBus.Receiver.Data
{
    public class ServiceBusDbContext : DbContext
    {
        public DbSet<Error> Errors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source = localhost; Initial Catalog = ServiceBusExampleDatabase; Integrated Security = True; ");
        }
    }
}
