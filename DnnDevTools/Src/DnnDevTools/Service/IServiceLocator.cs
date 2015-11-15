using weweave.DnnDevTools.Service.Config;
using weweave.DnnDevTools.Service.Dnn;
using weweave.DnnDevTools.Service.Settings;

namespace weweave.DnnDevTools.Service
{
    interface IServiceLocator
    {
        ISettingsService SettingsService { get; }

        IDnnService DnnService { get; }
        IConfigService ConfigService { get; }
    }
}
