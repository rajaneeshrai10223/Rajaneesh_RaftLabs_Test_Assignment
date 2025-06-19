using System.Threading.Tasks;

namespace User.Processing.Service.Interface
{
    public interface ICachingService
    {
        Task SetAsync(string key, object value, System.TimeSpan? absoluteExpiration = null);
        Task<T?> GetAsync<T>(string key);
    }
}