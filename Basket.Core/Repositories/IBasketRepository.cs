using Basket.Core.Entities;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {
        public Task<ServiceResult> GetBasket(string username);
        public Task<ServiceResult> UpdateBasket(ShoppingCart shoppingCart);
        public Task<ServiceResult> DeleteBasket(string username);
    }
}
