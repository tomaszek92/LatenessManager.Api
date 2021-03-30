using System;
using System.Security.Claims;
using LatenessManager.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace LatenessManager.Infrastructure.Services
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int Id
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (String.IsNullOrWhiteSpace(claim))
                {
                    throw new Exception("Claim is null");
                }
                
                return int.Parse(claim);
            }
        }
        
    }
}