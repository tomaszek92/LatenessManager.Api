using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace LatenessManager.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, (string, string)[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(
                    e => e.PropertyName,
                    e => (e.ErrorCode, e.ErrorMessage))
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, (string ErrorCode, string ErrorMessage)[]> Errors { get; }
    }
}