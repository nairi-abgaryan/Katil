using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Repositories.SystemSettings
{
    public class SystemSettingsRepository : BaseRepository<Model.SystemSettings>, ISystemSettingsRepository
    {
        public SystemSettingsRepository(KatilContext context)
            : base(context)
        {
        }

        public async Task<Model.SystemSettings> GetSetting(string key)
        {
            var setting = await Context.SystemSettings.SingleOrDefaultAsync(s => s.Key == key);
            return setting;
        }
    }
}
