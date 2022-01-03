using CreateCustomer.Model;
using Microsoft.EntityFrameworkCore;

namespace CreateCustomer.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options): base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
