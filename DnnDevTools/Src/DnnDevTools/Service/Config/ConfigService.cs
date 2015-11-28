using System.Net.Configuration;
using System.Net.Mail;
using System.Threading;
using System.Web.Configuration;
using weweave.DnnDevTools.SignalR;

namespace weweave.DnnDevTools.Service.Config
{
    internal class ConfigService : ServiceBase, IConfigService
    {
        public ConfigService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public bool SetEnableMailCatch(bool enableMailCatch)
        {
            var updated = ServiceLocator.SettingsService.UpdateSetting("EnableMailCatch", enableMailCatch.ToString());

            // Do nothing if config has not changed
            if (!updated) return false;

            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var section = configuration.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            var saveConfig = false;

            if (enableMailCatch)
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
                ServiceLocator.SettingsService.GetSetting("EnableMailCatch", false.ToString()),
                out enableMailCatch) && enableMailCatch;
        }

    }
}
