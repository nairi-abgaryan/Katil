using System.Threading.Tasks;
using Katil.Data.Repositories.Base;

namespace Katil.Data.Repositories.SystemSettings
{
    public interface ISystemSettingsRepository : IRepository<Model.SystemSettings>
    {
        Task<Model.SystemSettings> GetSetting(string key);
    }
}
