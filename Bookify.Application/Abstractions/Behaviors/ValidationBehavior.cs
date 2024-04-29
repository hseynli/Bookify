﻿using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Bookify.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any())         
            return await next();

        ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(v => v.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(result => result.Errors)
            .Select(failure => new ValidationError(failure.PropertyName, failure.ErrorMessage))
            .ToList();

        if(validationErrors.Any())
            throw new Exceptions.ValidationException(validationErrors);

        return await next();
    }
}
