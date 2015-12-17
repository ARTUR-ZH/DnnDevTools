using System.Net.Configuration;
using System.Net.Mail;
using System.Threading;
using System.Web.Configuration;
using log4net.Core;
using weweave.DnnDevTools.SignalR;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.Service.Config
{
    internal class ConfigService : ServiceBase, IConfigService
    {
        public ConfigService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public bool SetEnable(bool status)
        {
            return ServiceLocator.SettingsService.UpdateSetting("Enable", status.ToString());
        }

        public bool GetEnable()
        {
            bool enable;
            return bool.TryParse(
                ServiceLocator.SettingsService.GetSetting("Enable", true.ToString()),
                out enable) && enable;
        }

        public bool SetEnableMailCatch(bool status)
        {
            var updated = ServiceLocator.SettingsService.UpdateSetting("EnableMailCatch", status.ToString());

            // Do nothing if config has not changed
            if (!updated) return false;

            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var section = configuration.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            var saveConfig = false;

            if (status)
            {
                if (section == null)
                {
                    var smtpSection = new SmtpSection();
                    smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation = MailPickupFolderWatcher.Path;
                    smtpSection.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    configuration.Sections.Add("system.net/mailSettings/smtp", smtpSection);
                    saveConfig = true;
                }
                else
                {
                    if (section.DeliveryMethod != SmtpDeliveryMethod.SpecifiedPickupDirectory)
                    {
                        section.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        saveConfig = true;
                    }
                    if (section.SpecifiedPickupDirectory.PickupDirectoryLocation != MailPickupFolderWatcher.Path)
                    {
                        section.SpecifiedPickupDirectory.PickupDirectoryLocation = MailPickupFolderWatcher.Path;
                        saveConfig = true;
                    }
                }
            }
            else
            {
                if (section?.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
                {
                    section.DeliveryMethod = SmtpDeliveryMethod.Network;
                    saveConfig = true;
                }
            }

            // Return false if web.config is already up to date
            if (!saveConfig) return false;

            // Update web.config
            new Thread(() =>
            {
                configuration.Save();
            }).Start();
            return true;
        }

        public bool GetEnableMailCatch()
        {
            bool enableMailCatch;
            return bool.TryParse(
                ServiceLocator.SettingsService.GetSetting("EnableMailCatch", true.ToString()),
                out enableMailCatch) && enableMailCatch;
        }

        public Level GetLogMessageLevel()
        {
            var log4NetLevel = Log4NetUtil.ParseLevel(ServiceLocator.SettingsService.GetSetting("ALL", true.ToString()));
            return log4NetLevel ?? Level.All;
        }

        public bool SetLogMessageLevel(Level level)
        {
            return ServiceLocator.SettingsService.UpdateSetting("LogMessageLevel", level.ToString());
        }
    }
}
