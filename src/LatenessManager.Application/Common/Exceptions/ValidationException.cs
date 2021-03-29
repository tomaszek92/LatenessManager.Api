using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using LatenessManager.Application.Common.Models;

namespace LatenessManager.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, ValidationError[]> Errors { get; }

        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, ValidationError[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(
                    e => e.PropertyName,
                    e => new ValidationError
                    {
                        Code = e.ErrorCode,
                        Message = e.ErrorMessage
                    })
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray());
        }
    }
}