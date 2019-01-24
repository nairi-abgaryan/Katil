using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Repositories.AppUser
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(KatilContext context)
            : base(context)
        {
        }

        public async Task<User> GetUser(string email, string password)
        {
            var user = await Context.Users
                    .SingleOrDefaultAsync(u => u.Email == email && u.Password == password)
                ;

            return user;
        }

        public async Task<User> GetUserWithFullInfo(int id)
        {
            var user = await Context.Users
                .Include(u => u.SystemUserRole)
                .SingleOrDefaultAsync(s => s.Id == id);

            return user;
        }

        public async Task<DateTime?> GetLastModifiedDate(int userId)
        {
            var lastModifiedDate = await Context.Users
                .Where(d => d.Id == userId)
                .Select(d => d.ModifiedDate).ToListAsync();

            return lastModifiedDate.FirstOrDefault();
        }

        public async Task<List<User>> GetInternalUsersWithRolesAsync()
        {
            var adminRole = await Context.SystemUserRoles
                .SingleOrDefaultAsync(r => r.RoleName == RoleNames.Admin);

            var internalUsers = await Context.Users
                .Where(u => u.SystemUserRoleId == adminRole.SystemUserRoleId)
                .ToListAsync();

            return internalUsers;
        }

        public async Task<List<User>> GetUsersByRole(int userRoleId)
        {
            var users = await Context.Users
                .Where(u => u.SystemUserRoleId == userRoleId)
                .ToListAsync();

            return users;
        }

        public async Task<User> GetAdminUser(int userId)
        {
            var user = await Context.Users.SingleOrDefaultAsync(u =>
                u.Id == userId && u.SystemUserRoleId == (int)Roles.StaffUser);

            return user;
        }

        public async Task<User> GetUserWithInternalRolesAsync(int userId)
        {
            var user = await Context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            return user;
        }
    }
}
