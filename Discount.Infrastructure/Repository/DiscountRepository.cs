using Discount.Core.Entities;
using Entities.Base;
using Microsoft.Extensions.Logging;
using NLog;

public class DiscountRepository :ServiceBase<DiscountRepository> , IDiscountRepository
{
    public DiscountRepository(ILogger<DiscountRepository> logger) : base(logger)
    { 
    }
    public Task<ServiceResult> CreateDiscount(Coupon coupon)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> DeleteDiscount(string productName)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> GetDiscount(string productName)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> UpdateDiscount(Coupon coupon)
    {
        throw new NotImplementedException();
    }
}
