using Katil.Business.Services.SystemSettingsService;
using Katil.Business.Services.TokenServices;

namespace Katil.Business.Services
{
    public abstract class KatilServiceBase
    {
        private readonly ITokenService tokenService;
        private readonly ISystemSettingsService settingsService;

        protected KatilServiceBase(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        protected KatilServiceBase(ITokenService tokenService, ISystemSettingsService settingsService)
        {
            this.tokenService = tokenService;
            this.settingsService = settingsService;
        }

        protected ISystemSettingsService SystemSettingsService => settingsService;

        protected ITokenService TokenService => tokenService;
    }
}
