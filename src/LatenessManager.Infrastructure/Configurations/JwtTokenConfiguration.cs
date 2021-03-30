namespace LatenessManager.Infrastructure.Configurations
{
    public class JwtTokenConfiguration
    {
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}