using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using AutoMapper;
using Katil.Business.Entities.Models.User;
using Katil.Business.Services.TokenServices;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Katil.Data.Repositories.UnitOfWork;

namespace Katil.Business.Services.UserServices
{
    public class UserService : KatilServiceBase, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, ITokenService tokenService)
            : base(tokenService)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> GetUser(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                return Mapper.Map<User, UserResponse>(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<User> GetSystemUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return user;
        }

        public async Task<List<UserResponse>> GetUsers()
        {
            var internalUsers = await _unitOfWork.UserRepository.GetInternalUsersWithRolesAsync();
            if (internalUsers != null)
            {
                return Mapper.Map<List<User>, List<UserResponse>>(internalUsers);
            }

            return null;
        }

        public async Task<User> GetUserWithFullInfo(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserWithFullInfo(id);
            return user;
        }

        public async Task<User> PatchSystemUser(User user)
        {
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Complete();
            return user;
        }

        public async Task<DateTime?> GetLastModifiedDateAsync(object userId)
        {
            var disputeLastModified = await _unitOfWork.UserRepository.GetLastModifiedDate((int)userId);
            return disputeLastModified;
        }

        public async Task<UserLoginResponse> CreateUser(UserLoginRequest request)
        {
            var newUser = Mapper.Map<UserLoginRequest, User>(request);
            newUser.Password = HashHelper.GetHash(request.Password);
            var result = await _unitOfWork.UserRepository.InsertAsync(newUser);
            await _unitOfWork.Complete();
            return Mapper.Map<User, UserLoginResponse>(result);
        }

        public async Task<User> GetNoTrackingSystemUser(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetNoTrackingByIdAsync(x => x.Id == userId);
            return user;
        }

        public async Task<User> PatchAsync(User user)
        {
            try
            {
                _unitOfWork.UserRepository.Attach(user);
                var result = await _unitOfWork.Complete();

                if (result == 1)
                {
                    return user;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }

            return null;
        }

        public async Task<bool> Reset(User user, string value)
        {
            try
            {
                user.Password = HashHelper.GetHash(value);
                _unitOfWork.UserRepository.Attach(user);
                var result = await _unitOfWork.Complete();

                return result == 1;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }

            return false;
        }

        public async Task<string> CheckUserUnique(string email, int systemUserRoleId)
        {
            var users = await _unitOfWork.UserRepository.GetUsersByRole(systemUserRoleId);

            var isEmailExists = users.Exists(x => x.Email == email);
            if (isEmailExists)
            {
                if (users.Find(x => x.Email == email).IsActive != null)
                {
                    return ApiReturnMessages.DuplicateEmailForRole;
                }

                return ApiReturnMessages.InactiveEmailExists;
            }

            var isUsernameExists = users.Exists(x => x.Email == email);
            if (isUsernameExists)
            {
                if (users.Find(x => x.Email == email).IsActive != null && users.Find(x => x.AccountMobile == email).IsActive.Value)
                {
                    return ApiReturnMessages.DuplicateUsernameForRole;
                }

                return ApiReturnMessages.InactiveUsernameExists;
            }

            return string.Empty;
        }

        public async Task<bool> UserIsAdmin(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetAdminUser(userId);
            return user != null;
        }

        public async Task<bool> UserExists(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}
