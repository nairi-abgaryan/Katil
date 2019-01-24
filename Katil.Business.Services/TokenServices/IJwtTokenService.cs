using System.Threading.Tasks;
using Katil.Data.Model;

namespace Katil.Business.Services.TokenServices
{
    public interface IJwtTokenService
    {
        string GetJwtToken(User user);
    }
}