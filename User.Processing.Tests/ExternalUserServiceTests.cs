using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using User.Processing;
using User.Processing.Data;
using User.Processing.Service;
using User.Processing.Service.Interface;
using Xunit;

namespace User.Processing.Tests
{
    public class ExternalUserServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IHttpClientFactoryService> _mockHttpClientFactoryService;
        private readonly ExternalUserService _service;
        private readonly string _host = "http://localhost";

        public ExternalUserServiceTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["RequesServer:Host"]).Returns(_host);
            _mockHttpClientFactoryService = new Mock<IHttpClientFactoryService>();
            _service = new ExternalUserService(_mockConfig.Object, _mockHttpClientFactoryService.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserData()
        {
            var expectedUser = new UserData
            {
                Data = new DataInfo
                {
                    Id = 1,
                    Email = "a@a.com"
                }
            };
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedUser)
                });
            var client = new HttpClient(handler.Object);
            _mockHttpClientFactoryService.Setup(f => f.CreateClientWithApiKey()).Returns(client);

            var result = await _service.GetUserByIdAsync(1);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedUser.Data.Id, result.Data.Id);
            Assert.Equal(expectedUser.Data.Email, result.Data.Email);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsPagedUsers()
        {
            var usersCollection = new UsersCollection
            {
                Page = 1,
                PerPage = 2,
                TotalPages = 1,
                Data = new List<DataInfo> { new DataInfo { Id = 1 }, new DataInfo { Id = 2 } }
            };
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(usersCollection)
                });
            var client = new HttpClient(handler.Object);
            _mockHttpClientFactoryService.Setup(f => f.CreateClientWithApiKey()).Returns(client);

            var result = await _service.GetAllUsersAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllUsersWithDelayAsync_ReturnsUsers()
        {
            var usersCollection = new UsersCollection
            {
                Data = new List<DataInfo> { new DataInfo { Id = 1 }, new DataInfo { Id = 2 } }
            };
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(usersCollection)
                });
            var client = new HttpClient(handler.Object);
            _mockHttpClientFactoryService.Setup(f => f.CreateClientWithApiKey()).Returns(client);

            var result = await _service.GetAllUsersWithDelayAsync(2);

            Assert.NotNull(result);
        }
    }
}
