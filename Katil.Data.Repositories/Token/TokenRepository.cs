using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Repositories.Token
{
    public class TokenRepository : BaseRepository<UserToken>, ITokenRepository
    {
        public TokenRepository(KatilContext context)
            : base(context)
        {
        }

        public UserToken GetToken(string authToken)
        {
            var token = Context.UserTokens.SingleOrDefault(t => t.AuthToken == authToken && t.ExpiresOn > DateTime.Now);
            return token;
        }

        public async Task<UserToken> GetTokenAsync(string authToken)
        {
            try
            {
                var token = await Context.UserTokens.SingleOrDefaultAsync(t => t.AuthToken == authToken && t.ExpiresOn > DateTime.Now);
                return token;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
