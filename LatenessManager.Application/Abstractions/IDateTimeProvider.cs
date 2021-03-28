using System;

namespace LatenessManager.Application.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}