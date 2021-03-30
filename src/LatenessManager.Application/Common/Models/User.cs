using System.Collections.Generic;

namespace LatenessManager.Application.Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<UserRole> Roles { get; set; } = new();
    }
}