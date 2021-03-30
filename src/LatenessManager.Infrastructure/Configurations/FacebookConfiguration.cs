namespace LatenessManager.Infrastructure.Configurations
{
    public class FacebookConfiguration
    {
        public bool IsEnabled { get; set; }
        public string AccessToken { get; set; }
        public string DestinationPostId { get; set; }
    }
}