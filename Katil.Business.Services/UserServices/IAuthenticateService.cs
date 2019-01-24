using System.Threading.Tasks;
using Katil.Data.Model;
using Microsoft.IdentityModel.Tokens;

namespace Katil.Business.Services.UserServices
{
    public interface IAuthenticateService
    {
        Task<User> Login(string username, string password);
    }
}
