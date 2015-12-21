using weweave.DnnDevTools.Service.Config;
using weweave.DnnDevTools.Service.Dnn;
using weweave.DnnDevTools.Service.DnnEvent;
using weweave.DnnDevTools.Service.Log;
using weweave.DnnDevTools.Service.Mail;
using weweave.DnnDevTools.Service.Settings;

namespace weweave.DnnDevTools.Service
{
    interface IServiceLocator
    {
        ISettingsService SettingsService { get; }

        IDnnService DnnService { get; }

        IConfigService ConfigService { get; }

        IMailService MailService { get; }

        ILogService LogService { get; }

        IDnnEventService DnnEventService { get; }

    }
}
