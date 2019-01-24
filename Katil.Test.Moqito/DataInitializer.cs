using System;
using System.Collections.Generic;
using System.Linq;

using Katil.Common.Utilities;
using Katil.Data.Model;

namespace Katil.Test.Moqito
{
    public class DataInitializer
    {
        private const double ExpireDuration = 900;

        /// <summary>
        /// Dummy Token
        /// </summary>
        /// <returns>UserToken</returns>
        public static UserToken GetToken()
        {
            var user = GetUser(1);

            var token = new UserToken()
            {
                AuthToken = Guid.NewGuid().ToString(),
                IssuedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddSeconds(400),
                Id = user.Id,
            };
            return token;
        }

        /// <summary>
        /// Dummy Token List
        /// </summary>
        /// <returns>IQueryable<UserToken></returns>
        public static IQueryable<UserToken> GetTokens()
        {
            var tokenList = new List<UserToken>
            {
                new UserToken()
                {
                    Id = 1,
                    AuthToken = Guid.NewGuid().ToString(),
                    IssuedOn = DateTime.Now,
                    ExpiresOn = DateTime.Now.AddSeconds(ExpireDuration),
                    SystemUserGuid = Guid.NewGuid()
                },
                new UserToken()
                {
                    Id = 2,
                    AuthToken = Guid.NewGuid().ToString(),
                    IssuedOn = DateTime.Now,
                    ExpiresOn = DateTime.Now.AddSeconds(ExpireDuration),
                    SystemUserGuid = Guid.NewGuid()
                },
                new UserToken()
                {
                    Id = 3,
                    AuthToken = Guid.NewGuid().ToString(),
                    IssuedOn = DateTime.Now,
                    ExpiresOn = DateTime.Now.AddSeconds(ExpireDuration),
                    SystemUserGuid = Guid.NewGuid()
                },
                new UserToken()
                {
                    Id = 4,
                    AuthToken = Guid.NewGuid().ToString(),
                    IssuedOn = DateTime.Now,
                    ExpiresOn = DateTime.Now.AddSeconds(ExpireDuration),
                    SystemUserGuid = Guid.NewGuid()
                },
                new UserToken()
                {
                    Id = 5,
                    AuthToken = Guid.NewGuid().ToString(),
                    IssuedOn = DateTime.Now,
                    ExpiresOn = DateTime.Now.AddSeconds(ExpireDuration),
                    SystemUserGuid = Guid.NewGuid()
                }
            }.AsQueryable();

            return tokenList;
        }

        /// <summary>
        /// Dummy User
        /// </summary>
        /// <param name="id">Dummy</param>
        /// <returns>SystemUser</returns>
        public static User GetUser(int id)
        {
            return GetUsers().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Dummy Users List
        /// </summary>
        /// <returns>List<SystemUser></returns>
        public static List<User> GetUsers()
        {
            var users = new List<User>
            {
                new User() { Id = 1, SystemUserRoleId = (int)Roles.StaffUser, SystemUserRole = GetRoles().FirstOrDefault(x => x.SystemUserRoleId == (int)Roles.StaffUser), Email = "Admin", FullName = "admin", Password = "admin", IsActive = true },
                new User() { Id = 2, SystemUserRoleId = (int)Roles.ExternalUser, SystemUserRole = GetRoles().FirstOrDefault(x => x.SystemUserRoleId == (int)Roles.ExternalUser), Email = "External User", FullName = "external", Password = "external", IsActive = false,  },
            };
            return users;
        }

        /// <summary>
        /// Dummy Roles
        /// </summary>
        /// <returns>List<SystemUserRole></returns>
        public static List<SystemUserRole> GetRoles()
        {
            var roles = new List<SystemUserRole>
            {
                new SystemUserRole() { SystemUserRoleId = (int)Roles.StaffUser, RoleName = "Staff User", RoleDescritption = "Internal Staff User - Able to view all dispute information", SessionDuration = 3600 },
                new SystemUserRole() { SystemUserRoleId = (int)Roles.ExternalUser, RoleName = "External User", RoleDescritption = "External Dispute Participant - Can only access disputes that they are associated to", SessionDuration = 900 },
            };

            return roles;
        }
    }
}