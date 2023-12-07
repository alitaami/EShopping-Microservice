using Microsoft.EntityFrameworkCore;
using Order.Core.Common;
using Order.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data
{
    public class OrderContext :DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) :base(options)  
        {

        }
        public DbSet<Order.Core.Entities.Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate =DateTime.Now;
                        entry.Entity.CreatedBy = "Ali"; //TODO: will be replaced by identity server
                        break;
                    
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate =DateTime.Now;
                        entry.Entity.LastModifiedBy = "Ali"; //TODO: will be replaced by identity server
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
