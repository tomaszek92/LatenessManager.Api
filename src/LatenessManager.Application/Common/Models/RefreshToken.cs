namespace LatenessManager.Application.Common.Models
{
    public class RefreshToken
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public bool Revoked { get; set; }
    }
}