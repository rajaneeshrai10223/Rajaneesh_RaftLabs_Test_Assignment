using System.Collections.Generic;
using System.Threading.Tasks;
using User.Processing.Data;

namespace User.Processing.Service.Interface
{
    public interface IExternalUserService
    {
        Task<UserData> GetUserByIdAsync(int userId);
        Task<IEnumerable<DataInfo>> GetAllUsersAsync();
    }
}