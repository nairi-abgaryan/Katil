using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;

namespace Katil.Data.Repositories.AppUser
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUser(string email, string password);

        Task<User> GetUserWithFullInfo(int id);

        Task<DateTime?> GetLastModifiedDate(int userId);

        Task<List<User>> GetInternalUsersWithRolesAsync();

        Task<List<User>> GetUsersByRole(int systemUserRoleId);

        Task<User> GetAdminUser(int userId);

        Task<User> GetUserWithInternalRolesAsync(int userId);
    }
}
