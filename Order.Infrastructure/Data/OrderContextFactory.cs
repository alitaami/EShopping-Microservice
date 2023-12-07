using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
           var optionBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionBuilder.UseSqlServer("Data Source = OrderDb");
            return new OrderContext(optionBuilder.Options);
        }
    }
}
