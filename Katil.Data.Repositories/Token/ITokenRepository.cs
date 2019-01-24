using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;

namespace Katil.Data.Repositories.Token
{
    public interface ITokenRepository : IRepository<UserToken>
    {
        UserToken GetToken(string authToken);

        Task<UserToken> GetTokenAsync(string authToken);
    }
}
