using Dapper;
using Discount.Common.Resources;
using Discount.Core.Entities;
using Entities.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using Npgsql;

public class DiscountRepository : ServiceBase<DiscountRepository>, IDiscountRepository
{
    private readonly IConfiguration _configuration;
    public DiscountRepository(IConfiguration configuration, ILogger<DiscountRepository> logger) : base(logger)
    {
        _configuration = configuration;
    }
    public async Task<ServiceResult> CreateDiscount(Coupon coupon)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName , Description , Amount) VALUES (@ProductName ,@Description , @Amount)",
                new{ ProductName = coupon.ProductName , Description = coupon.Description , Amount = coupon.Amount });

            if (affected == 0)
                return BadRequest(ErrorCodeEnum.BadRequest, Resource.CreateError, null);///

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, null, null);

            return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
        }
    }

    public async Task<ServiceResult> DeleteDiscount(string productName)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                 await connection.ExecuteAsync
                 ("DELETE FROM Coupon WHERE ProductName = @ProductName",
                 new { ProductName = productName });

            if (affected == 0)
                return BadRequest(ErrorCodeEnum.BadRequest, Resource.DeleteError, null);///

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, null, null);

            return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
        }
    }

    public async Task<ServiceResult> GetDiscount(string productName)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
                return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

            return Ok(coupon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, null, null);

            return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
        }
    }

    public async Task<ServiceResult> UpdateDiscount(Coupon coupon)
    {
         try
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                 ("UPDATE Coupon SET ProductName = @ProductName , Description =@Description , Amount =@Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return BadRequest(ErrorCodeEnum.BadRequest, Resource.UpdateError, null);///

            return Ok(coupon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, null, null);

            return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
        }
    }
}
