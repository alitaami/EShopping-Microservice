using Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public class ExceptionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> _logger;

    public ExceptionPipelineBehavior(ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            // Continue with the next behavior or handler in the pipeline
            return await next();
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An exception occurred during the request handling.");

            // Handle the exception and convert it to ServiceResult with ApiResult
            var serviceResult = HandleException(ex);

            // Return the ServiceResult as the response
            return (TResponse)(object)serviceResult;
        }
    }

    private ServiceResult HandleException(Exception exception)
    {
        // Log the exception or perform any other necessary actions

        // Convert the exception to ApiResult
        var apiResult = ConvertExceptionToApiResult(exception);

        // Create a ServiceResult with the ApiResult
        var serviceResult = new ServiceResult(null, apiResult);

        return serviceResult;
    }

    private ApiResult ConvertExceptionToApiResult(Exception exception)
    {
        // Handle other types of exceptions here...

        // For unhandled exceptions, return a generic error
        return new ApiResult(HttpStatusCode.InternalServerError, ErrorCodeEnum.InternalError, exception.Message, null);
    }
}
