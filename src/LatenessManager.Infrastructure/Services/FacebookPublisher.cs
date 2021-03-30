using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;
using LatenessManager.Infrastructure.Configurations;

namespace LatenessManager.Infrastructure.Services
{
    public class FacebookPublisher : IFacebookPublisher
    {
        private readonly IFacebookClient _facebookClient;
        private readonly FacebookConfiguration _facebookConfiguration;

        public FacebookPublisher(IFacebookClient facebookClient, FacebookConfiguration facebookConfiguration)
        {
            _facebookClient = facebookClient;
            _facebookConfiguration = facebookConfiguration;
        }

        public async Task PublishCommentAsync(string message, CancellationToken cancellationToken)
        {
            if (!_facebookConfiguration.IsEnabled)
            {
                return;
            }
            
            var accessToken = _facebookConfiguration.AccessToken;
            var endpoint = $"{_facebookConfiguration.DestinationPostId}/comments";
            var data = new {message};
            await _facebookClient.PostAsync(accessToken, endpoint, data, null, cancellationToken);
        }
    }
}