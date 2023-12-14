using Application.Features.Behavior.Contracts;
using Entities.Base;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Behavior
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IValidatable
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        // validate errors with any request that has been sent
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task
                    .WhenAll(_validators
                    .Select(s => s.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(s => s.Errors)
                    .Where(e => e != null).ToList();

                if (failures.Count != 0)
                {
                    var errorItems = failures.Select(failure => new FieldErrorItem(failure.PropertyName, new List<string> { failure.ErrorMessage })).ToList();
                    var apiResult = new ApiResult(System.Net.HttpStatusCode.BadRequest, ErrorCodeEnum.NullField, "One or more validation failures occurred", errorItems);
                    return (TResponse)(object)new ServiceResult(null, apiResult);
                }
            }
            return await next();
        }
    }
}
