using System.Threading.Tasks;
using Katil.Business.Entities.Models.User;
using Katil.Data.Model;

namespace Katil.Business.Services.TokenServices
{
    public interface ITokenService
    {
        Task<UserToken> GenerateToken(int sessionDuration, int? userId, int? participantId = null);

        Task<bool> ValidateToken(string authToken, bool extendSession = true);

        Task<bool> KillToken(string authToken);

        Task<int> GetUserId(string authToken);

        Task<UserToken> GetUserToken(string authToken);

        Task<SessionResponse> ExtendSession(string authToken);
    }
}
