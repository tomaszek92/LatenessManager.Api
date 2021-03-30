using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.Results;
using LatenessManager.Application.Common.Exceptions;
using Xunit;

namespace LatenessManager.Application.UnitTests.Common.Exceptions
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void creates_empty_error_dictionary_by_default_constructor()
        {
            var actual = new ValidationException().Errors;

            actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
        }

        [Fact]
        public void creates_single_element_error_dictionary()
        {
            var failures = new List<ValidationFailure>
            {
                CreateValidationFailure("Age", "InvalidAge", "must be over 18"),
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo("Age");

            actual["Age"].Should().BeEquivalentTo(new object[]
            {
                new {Code = "InvalidAge", Message = "must be over 18"}
            });
        }

        [Fact]
        public void creates_multiple_element_error_dictionary_each_with_multiple_values()
        {
            var failures = new List<ValidationFailure>
            {
                CreateValidationFailure("Age", "InvalidAge1", "must be 18 or older"),
                CreateValidationFailure("Age", "InvalidAge2", "must be 25 or younger"),
                CreateValidationFailure("Password", "InvalidPassword1", "must contain at least 8 characters"),
                CreateValidationFailure("Password", "InvalidPassword2", "must contain a digit"),
                CreateValidationFailure("Password", "InvalidPassword3", "must contain upper case letter"),
                CreateValidationFailure("Password", "InvalidPassword4", "must contain lower case letter"),
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo("Password", "Age");

            actual["Age"].Should().BeEquivalentTo(
                new {Code = "InvalidAge1", Message = "must be 18 or older"},
                new {Code = "InvalidAge2", Message = "must be 25 or younger"});

            actual["Password"].Should().BeEquivalentTo(
                new {Code = "InvalidPassword1", Message = "must contain at least 8 characters"},
                new {Code = "InvalidPassword2", Message = "must contain a digit"},
                new {Code = "InvalidPassword3", Message = "must contain upper case letter"},
                new {Code = "InvalidPassword4", Message = "must contain lower case letter"});
        }

        private static ValidationFailure CreateValidationFailure(string propertyName, string code, string message) =>
            new(propertyName, message) {ErrorCode = code};
    }
}