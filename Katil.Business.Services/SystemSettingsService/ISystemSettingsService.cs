using System.Collections.Generic;
using System.Threading.Tasks;
using Katil.Business.Entities.Models.Setting;
using Katil.Data.Model;

namespace Katil.Business.Services.SystemSettingsService
{
    public interface ISystemSettingsService
    {
        Task<SystemSettings> GetSetting(string key);

        Task<bool> UpdateSettingValue(string key, string value);

        Task<List<SettingResponse>> GetSettingsAsync();
    }
}
