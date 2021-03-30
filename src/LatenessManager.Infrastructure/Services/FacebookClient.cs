using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LatenessManager.Application.Abstractions;

namespace LatenessManager.Infrastructure.Services
{
    public class FacebookClient : IFacebookClient
    {
        private readonly HttpClient _httpClient;

        public FacebookClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://graph.facebook.com/v10.0/");
            
            httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Get request failed");
            }
 
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
 
            return JsonSerializer.Deserialize<T>(result);
        }
        
        public async Task PostAsync(string accessToken, string endpoint, object data, string args = null, CancellationToken cancellationToken = default)
        {
            var payload = GetPayload(data);
            var response = await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Post request failed");
            }
        }

        private static StringContent GetPayload(object data)
        {
            var json = JsonSerializer.Serialize(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}