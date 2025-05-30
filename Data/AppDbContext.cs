using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderManagement.Models;

namespace OrderManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var customerSegmentConverter = new ValueConverter<CustomerSegment, string>(
                v => v.ToString(),
                v => (CustomerSegment)Enum.Parse(typeof(CustomerSegment), v));

            var orderStatusConverter = new ValueConverter<OrderStatus, string>(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

            modelBuilder.Entity<Customer>()
                .Property(c => c.Segment)
                .HasConversion(customerSegmentConverter);

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion(orderStatusConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
