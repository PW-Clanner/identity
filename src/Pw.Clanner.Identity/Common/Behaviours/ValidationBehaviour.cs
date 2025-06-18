using FluentValidation;
using MediatR;
using Pw.Clanner.Identity.Common.Exceptions;

namespace Pw.Clanner.Identity.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationTasks = validators.Select(x => x.ValidateAsync(context, cancellationToken));

            var validationResults = await Task.WhenAll(validationTasks);

            var failures = validationResults
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Count > 0)
            {
                throw new AppValidationException(failures);
            }
        }

        return await next(cancellationToken);
    }
}