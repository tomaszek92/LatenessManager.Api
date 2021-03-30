using LatenessManager.Application.Common.Models;

namespace LatenessManager.Application.Identity.Abstractions
{
    public interface IJwtHandler
    {
        JsonWebToken Create(int userId, string[] roles);
    }
}