using System.Net.Http;

namespace User.Processing.Service.Interface
{
    public interface IHttpClientFactoryService
    {
        HttpClient CreateClientWithApiKey();
    }
}