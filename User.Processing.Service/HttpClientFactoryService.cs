using System.Net.Http;
using Microsoft.Extensions.Configuration;
using User.Processing.Service.Interface;

namespace User.Processing.Service
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiKey = _configuration["RequesServer:ApiKey"] ?? string.Empty;
        }

        public HttpClient CreateClientWithApiKey()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            return client;
        }
    }
}