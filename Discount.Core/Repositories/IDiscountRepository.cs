using Discount.Core.Entities;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IDiscountRepository
{
    Task<ServiceResult> GetDiscount(string productName);
    Task<ServiceResult> CreateDiscount(Coupon coupon);
    Task<ServiceResult> UpdateDiscount(Coupon coupon);
    Task<ServiceResult> DeleteDiscount(string productName);
}
