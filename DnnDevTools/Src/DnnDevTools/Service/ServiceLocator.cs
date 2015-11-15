using System.Configuration.Internal;
using weweave.DnnDevTools.Service.Config;
using weweave.DnnDevTools.Service.Dnn;
using weweave.DnnDevTools.Service.Settings;

namespace weweave.DnnDevTools.Service
{
    class ServiceLocator : IServiceLocator
    {
        private ISettingsService _settingsService;
        public ISettingsService SettingsService
        {
            get
            {
                if (_settingsService != null) return _settingsService;
                _settingsService = new SettingsService(this);
                return _settingsService;
            }
        }

        private IDnnService _dnnService;
        public IDnnService DnnService
        {
            get
            {
                if (_dnnService != null) return _dnnService;
                _dnnService = new DnnService(this);
                return _dnnService;
            }
        }

        private IConfigService _configService;
        public IConfigService ConfigService
        {
            get
            {
                if (_configService != null) return _configService;
                _configService = new ConfigService(this);
                return _configService;
            }
        }
    }
}
