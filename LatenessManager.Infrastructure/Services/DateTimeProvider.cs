using System;
using LatenessManager.Application.Abstractions;

namespace LatenessManager.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}