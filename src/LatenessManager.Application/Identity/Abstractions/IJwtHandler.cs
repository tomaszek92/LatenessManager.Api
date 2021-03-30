using LatenessManager.Application.Common.Models;
using LatenessManager.Application.Identity.Dtos;

namespace LatenessManager.Application.Identity.Abstractions
{
    public interface IJwtHandler
    {
        JsonWebTokenDto Create(int userId, string[] roles);
    }
}