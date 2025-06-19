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

        public async Task<IEnumerable<DataInfoWithPaging>> GetAllUsersAsync()
        {
            var client = _httpClientFactoryService.CreateClientWithApiKey();

            List<DataInfoWithPaging> allUsersWithPaging = new();

            int currentPage = 1;
            int totalPages = 1;
            do
            {
                var response = await client.GetAsync($"{_host}/api/users?page={totalPages}");
                response.EnsureSuccessStatusCode();
                var usersCollection = await response.Content.ReadFromJsonAsync<UsersCollection>();
                if (usersCollection != null)
                {
                    DataInfoWithPaging usersWithPaging = new DataInfoWithPaging
                    {
                        Page = usersCollection.Page,
                        PerPage = usersCollection.PerPage,
                        TotalPages = usersCollection.TotalPages,
                        Data = usersCollection.Data
                    };
                    allUsersWithPaging!.AddRange(usersWithPaging);

                    totalPages = usersCollection.TotalPages;
                    currentPage++;
                    
                    // If the current page is the last page, break the loop
                    if (currentPage > totalPages)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            while (currentPage <= totalPages);

            return allUsersWithPaging;
        }

        public async Task<IEnumerable<DataInfo>> GetAllUsersWithDelayAsync(int delay)
        {
            var client = _httpClientFactoryService.CreateClientWithApiKey();
            var response = await client.GetAsync($"{_host}/api/users?delay={delay}");
            response.EnsureSuccessStatusCode();
            var usersCollection = await response.Content.ReadFromJsonAsync<UsersCollection>();
            return usersCollection?.Data ?? Enumerable.Empty<DataInfo>();
        }

        
    }
}