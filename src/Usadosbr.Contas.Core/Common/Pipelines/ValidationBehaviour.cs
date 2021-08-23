using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Usadosbr.Contas.Core.Common.Result;

namespace Usadosbr.Contas.Core.Common.Pipelines
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        private static ConcurrentDictionary<Type, Func<ValidationError[], TResponse>> _cache = new();

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var results =
                    await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = results.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var validations = ToValidationErrors(failures);

                    if (_cache.TryGetValue(typeof(TResponse), out var invalid))
                    {
                        return invalid(validations);
                    }

                    var method = typeof(TResponse).GetMethod("Invalid", new[] {typeof(ValidationError[])})!;

                    var @delegate =
                        (Func<ValidationError[], TResponse>) method.CreateDelegate(
                            typeof(Func<ValidationError[], TResponse>));

                    _cache.TryAdd(typeof(TResponse), @delegate);

                    return @delegate(validations);
                }
            }

            return await next();
        }

        private static ValidationError[] ToValidationErrors(IEnumerable<ValidationFailure> failures)
        {
            return failures
                .GroupBy(x => x.PropertyName)
                .Select(y => new ValidationError(y.Key, y.Select(z => z.ErrorMessage).ToArray())).ToArray();
        }
    }
}