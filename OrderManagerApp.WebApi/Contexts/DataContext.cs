using Microsoft.EntityFrameworkCore;
using OrderManagerApp.WebApi.Models.Entities;

namespace OrderManagerApp.WebApi.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderRowEntity>().HasKey(or => new { or.OrderId, or.ProductId });
        }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderRowEntity> OrdersRows { get; set; }
    }
}
