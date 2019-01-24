using System.Threading.Tasks;
using Katil.Data.Repositories.AppUser;
using Katil.Data.Repositories.Files;
using Katil.Data.Repositories.SystemSettings;
using Katil.Data.Repositories.Token;

namespace Katil.Data.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        ITokenRepository TokenRepository { get; }

        IUserRepository UserRepository { get; }

        ISystemSettingsRepository SystemSettingsRepository { get; }

        IFileRepository FileRepository { get; }

        Task<int> Complete();
    }
}
