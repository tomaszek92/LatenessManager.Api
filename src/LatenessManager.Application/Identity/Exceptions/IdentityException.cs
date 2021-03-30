using System;

namespace LatenessManager.Application.Identity.Exceptions
{
    public class IdentityException : Exception
    {
        public IdentityException(string message) : base(message)
        {
        }
    }
}