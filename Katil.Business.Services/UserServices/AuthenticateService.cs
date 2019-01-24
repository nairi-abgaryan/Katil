using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Katil.Business.Entities.Models.User;
using Katil.Business.Services.TokenServices;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Katil.Data.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;

namespace Katil.Business.Services.UserServices
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticateService(IUnitOfWork unitOfWork, ITokenService tokenService, IJwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _unitOfWork.UserRepository.GetUser(username, HashHelper.GetHash(password));

            if (user != null)
            {
                var token = _jwtTokenService.GetJwtToken(user);
                user.Token = token;

                return user;
            }

            return null;
        }
    }
}
