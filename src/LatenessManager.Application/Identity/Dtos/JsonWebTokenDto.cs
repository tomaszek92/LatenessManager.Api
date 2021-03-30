namespace LatenessManager.Application.Identity.Dtos
{
    public class JsonWebTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}