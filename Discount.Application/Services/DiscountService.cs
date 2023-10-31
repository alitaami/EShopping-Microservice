using AutoMapper;
using Discount.Application.Features.Commands;
using Discount.Application.Features.Queries;
using Discount.Core.ViewModels;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Api.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DiscountService> _logger;
    //private readonly ICorrelationIdGenerator _correlationIdGenerator;
    private readonly IMapper _mapper;

    public DiscountService(IMapper mapper, IMediator mediator, ILogger<DiscountService> logger/*, ICorrelationIdGenerator correlationIdGenerator*/)
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
        //_correlationIdGenerator = correlationIdGenerator;
        //_logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        try
        {
            var result = await _mediator.Send(new GetDiscountQuery(request.ProductName));

            var data = _mapper.Map<CouponModel>(result);

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error occured in GetDiscount");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred.", ex));
        }
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        try
        {
            var model = _mapper.Map<CouponViewModel>(request);

            var result = await _mediator.Send(new CreateDiscountCommand(model));

            var res = _mapper.Map<CouponModel>(result);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error occured in GetDiscount");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred.",ex));
        }
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        try
        {
            var model = _mapper.Map<CouponUpdateViewModel>(request);

            var result = await _mediator.Send(new UpdateDiscountCommand(model));

            var res = _mapper.Map<CouponModel>(result);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error occured in GetDiscount");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred.", ex));
        }
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        try
        {
            var result = await _mediator.Send(new DeleteDiscountCommand(request.ProductName));

            var res = _mapper.Map<DeleteDiscountResponse>(result);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error occured in GetDiscount");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred.", ex));
        }
    }
}