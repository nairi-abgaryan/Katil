using System.Collections.Generic;
using System.Threading.Tasks;
using Katil.Business.Entities.Models.User;
using Katil.Data.Model;

namespace Katil.Business.Services.UserServices
{
    public interface IUserService : IServiceBase
    {
        Task<User> GetUserWithFullInfo(int id);

        Task<UserResponse> GetUser(int id);

        Task<List<UserResponse>> GetUsers();

        Task<User> GetSystemUser(int id);

        Task<User> PatchSystemUser(User user);

        Task<UserLoginResponse> CreateUser(UserLoginRequest request);

        Task<User> GetNoTrackingSystemUser(int userId);

        Task<User> PatchAsync(User user);

        Task<bool> Reset(User user, string value);

        Task<bool> UserExists(int userId);
    }
}
