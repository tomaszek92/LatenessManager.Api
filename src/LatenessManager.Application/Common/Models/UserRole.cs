namespace LatenessManager.Application.Common.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Role { get; set; }
    }
}