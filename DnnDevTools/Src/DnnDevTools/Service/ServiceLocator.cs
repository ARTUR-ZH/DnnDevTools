using System.Configuration.Internal;
using weweave.DnnDevTools.Service.Config;
using weweave.DnnDevTools.Service.Dnn;
using weweave.DnnDevTools.Service.DnnEvent;
using weweave.DnnDevTools.Service.Log;
using weweave.DnnDevTools.Service.Mail;
using weweave.DnnDevTools.Service.Settings;

namespace weweave.DnnDevTools.Service
{
    public class ServiceLocator : IServiceLocator
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

        private IMailService _mailService;
        public IMailService MailService
        {
            get
            {
                if (_mailService != null) return _mailService;
                _mailService = new MailService(this);
                return _mailService;
            }
        }

        private IDnnEventService _dnnEventService;
        public IDnnEventService DnnEventService
        {
            get
            {
                if (_dnnEventService != null) return _dnnEventService;
                _dnnEventService = new DnnEventService(this);
                return _dnnEventService;
            }
        }

        private ILogService _logService;
        public ILogService LogService
        {
            get
            {
                if (_logService != null) return _logService;
                _logService = new LogService(this);
                return _logService;
            }
        }
    }
}
