using System;
using System.Threading.Tasks;
using Katil.Business.Entities.Models.User;
using Katil.Data.Model;
using Katil.Data.Repositories.UnitOfWork;

namespace Katil.Business.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserToken> GenerateToken(int sessionDuration, int? userId, int? participantId = null)
        {
            var systemUser = userId.HasValue ? await _unitOfWork.UserRepository.GetByIdAsync(userId.Value) : null;

            var token = new UserToken()
            {
                AuthToken = Guid.NewGuid().ToString(),
                IssuedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddSeconds(sessionDuration),
                Id = userId,
                ParticipantId = participantId
            };

            await _unitOfWork.TokenRepository.InsertAsync(token);
            await _unitOfWork.Complete();

            return token;
        }

        public async Task<bool> ValidateToken(string authToken, bool extendSession = true)
        {
            var token = await _unitOfWork.TokenRepository.GetTokenAsync(authToken);
            if (token != null)
            {
                if (extendSession)
                {
                    token.ExpiresOn = DateTime.Now.AddSeconds(10);
                }

                _unitOfWork.TokenRepository.Update(token);
                await _unitOfWork.Complete();
                return true;
            }

            return false;
        }

        public async Task<bool> KillToken(string authToken)
        {
            var token = await _unitOfWork.TokenRepository.GetTokenAsync(authToken);
            if (token != null)
            {
                token.ExpiresOn = DateTime.Now;
                _unitOfWork.TokenRepository.Update(token);
                await _unitOfWork.Complete();
                return true;
            }

            return false;
        }

        public async Task<int> GetUserId(string authToken)
        {
            var token = await _unitOfWork.TokenRepository.GetTokenAsync(authToken);
            if (token != null && token.Id.HasValue)
            {
                var user = await _unitOfWork.UserRepository.GetNoTrackingByIdAsync(x => x.Id == token.Id.Value);
                return user.Id;
            }

            return 0;
        }

        public async Task<UserToken> GetUserToken(string authToken)
        {
            var token = await _unitOfWork.TokenRepository.GetTokenAsync(authToken);

            return token;
        }

        public async Task<SessionResponse> GetSessionDuration(string authToken)
        {
            var token = await _unitOfWork.TokenRepository.GetTokenAsync(authToken);
            if (token != null)
            {
                var sessionTimeRemaining = (int)token.ExpiresOn.Subtract(DateTime.Now).TotalSeconds;
                return new SessionResponse() { SessionTimeRemaining = sessionTimeRemaining };
            }

            return null;
        }

        public Task<SessionResponse> ExtendSession(string authToken)
        {
            throw new NotImplementedException();
        }
    }
}