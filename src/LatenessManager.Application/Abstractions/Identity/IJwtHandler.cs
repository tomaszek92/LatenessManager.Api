using LatenessManager.Application.Common.Models;

namespace LatenessManager.Application.Abstractions.Identity
{
    public interface IJwtHandler
    {
        JsonWebToken Create(int userId, string[] roles);
    }
}