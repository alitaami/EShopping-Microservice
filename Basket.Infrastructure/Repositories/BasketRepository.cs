using Basket.Common.Resources;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Entities.Base;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : ServiceBase<BasketRepository>, IBasketRepository
    {
        IDistributedCache _redis;
        public BasketRepository(IDistributedCache redis, ILogger<BasketRepository> logger) : base(logger)
        {
            _redis = redis;
        }

        public async Task<ServiceResult> DeleteBasket(string username)
        {
            try
            {
               await _redis.RemoveAsync(username);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> GetBasket(string username)
        {
            try
            {
                var basket = await _redis.GetStringAsync(username);

                if (basket is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                var res = JsonConvert.DeserializeObject<ShoppingCart>(basket);

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> UpdateBasket(ShoppingCart shoppingCart)
        {
            try
            {
                var serializedData = JsonConvert.SerializeObject(shoppingCart);
                
                if(serializedData is null)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.GeneralErrorTryAgain, null);///

                await _redis.SetStringAsync(shoppingCart.Username,serializedData);

                return await GetBasket(shoppingCart.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, ex.Message, null);
            }
        }
    }
}
