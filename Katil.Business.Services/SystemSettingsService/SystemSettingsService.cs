using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Katil.Business.Entities.Models.Setting;
using Katil.Data.Model;
using Katil.Data.Repositories.UnitOfWork;

namespace Katil.Business.Services.SystemSettingsService
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemSettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SystemSettings> GetSetting(string key)
        {
            var setting = await _unitOfWork.SystemSettingsRepository.GetSetting(key);
            return setting;
        }

        public async Task<List<SettingResponse>> GetSettingsAsync()
        {
            var settings = await _unitOfWork.SystemSettingsRepository.GetAllAsync();
            return Mapper.Map<List<SettingResponse>>(settings.OrderBy(s => s.SystemSettingsId));
        }

        public async Task<bool> UpdateSettingValue(string key, string value)
        {
            var setting = await _unitOfWork.SystemSettingsRepository.GetSetting(key);
            setting.Value = value;

            var result = await _unitOfWork.Complete();

            if (result == 1)
            {
                return true;
            }

            return false;
        }
    }
}
