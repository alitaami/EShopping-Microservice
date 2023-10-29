using Discount.Core.Entities;

public class DiscountRepository : IDiscountRepository
{
    public Task<bool> CreateDiscount(Coupon coupon)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteDiscount(string productName)
    {
        throw new NotImplementedException();
    }

    public Task<Coupon> GetDiscount(string productName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateDiscount(Coupon coupon)
    {
        throw new NotImplementedException();
    }
}
