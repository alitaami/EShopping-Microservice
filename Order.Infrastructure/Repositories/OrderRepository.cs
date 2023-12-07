using Entities.Base;
using EstateAgentApi.Services.Base;
using Microsoft.Extensions.Logging;
using Order.Common.Resources;
using Order.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : ServiceBase<OrderRepository>, IOrderRepository
    {
        private IAsyncRepository<Order.Core.Entities.Order> _db;
        public OrderRepository(IAsyncRepository<Core.Entities.Order> db, ILogger<OrderRepository> logger) : base(logger)
        {
            _db = db;
        }

        public async Task<ServiceResult> GetOrdersByUserName(string userName)
        {
            try
            {
                var orders = await _db.GetAllAsync(c => c.UserName == userName);
                return Ok(orders); // Assuming Ok is a method in your base class to create a successful result
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
    }
}
