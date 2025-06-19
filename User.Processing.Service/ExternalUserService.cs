using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using User.Processing.Data;
using User.Processing.Service.Interface;

namespace User.Processing.Service
{
    public class ExternalUserService : IExternalUserService
    {
        private readonly IConfiguration _configuration;
        private readonly string _host;
        private readonly IHttpClientFactoryService _httpClientFactoryService;

        public ExternalUserService(IConfiguration configuration, IHttpClientFactoryService httpClientFactoryService)
        {
            _configuration = configuration;
            _host = _configuration["RequesServer:Host"] ?? string.Empty;
            _httpClientFactoryService = httpClientFactoryService;
        }

        public async Task<UserData> GetUserByIdAsync(int userId)
        {
            var client = _httpClientFactoryService.CreateClientWithApiKey();
            var response = await client.GetAsync($"{_host}/api/users/{userId}");
            response.EnsureSuccessStatusCode();
            var userData = await response.Content.ReadFromJsonAsync<UserData>();
            return userData!;
        }

        public async Task<IEnumerable<DataInfo>> GetAllUsersAsync()
        {
            var client = _httpClientFactoryService.CreateClientWithApiKey();
            var response = await client.GetAsync($"{_host}/api/users");
            response.EnsureSuccessStatusCode();
            var usersCollection = await response.Content.ReadFromJsonAsync<UsersCollection>();
            return usersCollection?.Data ?? Enumerable.Empty<DataInfo>();
        }
    }
}