using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Order.Infrastructure.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionsBuilder.UseSqlServer("Data Source=OrderDb");
            return new OrderContext(optionsBuilder.Options);
        }
    }
}
