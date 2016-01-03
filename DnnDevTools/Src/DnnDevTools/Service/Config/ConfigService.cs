using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading;
using System.Web.Configuration;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using log4net.Core;
using weweave.DnnDevTools.SignalR;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.Service.Config
{
    internal class ConfigService : ServiceBase, IConfigService
    {
        private const string ConfigKeyEnable = "Enable";
        private const string ConfigKeyEnableMailCatch = "EnableMailCatch";
        private const string ConfigKeyEnableDnnEventTrace = "DnnEventTrace";
        private const string ConfigKeyLogMessageTraceLevel = "LogMessageTraceLevel";
        private const string ConfigKeyAllowedRoles = "AllowedRoles";

        public ConfigService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public bool SetEnable(bool status)
        {
            return ServiceLocator.SettingsService.UpdateSetting(ConfigKeyEnable, status.ToString());
        }

        public bool GetEnable()
        {
            bool enable;
            return bool.TryParse(
                ServiceLocator.SettingsService.GetSetting(ConfigKeyEnable, true.ToString()),
                out enable) && enable;
        }

        public bool SetEnableMailCatch(bool status)
        {
            var updated = ServiceLocator.SettingsService.UpdateSetting(ConfigKeyEnableMailCatch, status.ToString());

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

            return updated;
        }

        public bool GetEnableMailCatch()
        {
            bool enableMailCatch;
            return bool.TryParse(
                ServiceLocator.SettingsService.GetSetting(ConfigKeyEnableMailCatch, false.ToString()),
                out enableMailCatch) && enableMailCatch;
        }

        public bool SetEnableDnnEventTrace(bool status)
        {
            return ServiceLocator.SettingsService.UpdateSetting(ConfigKeyEnableDnnEventTrace, status.ToString());
        }

        public bool GetEnableDnnEventTrace()
        {
            bool enableMailCatch;
            return bool.TryParse(
                ServiceLocator.SettingsService.GetSetting(ConfigKeyEnableDnnEventTrace, true.ToString()),
                out enableMailCatch) && enableMailCatch;
        }

        public Level GetLogMessageTraceLevel()
        {
            var log4NetLevel = Log4NetUtil.ParseLevel(ServiceLocator.SettingsService.GetSetting(ConfigKeyLogMessageTraceLevel, Level.All.ToString()));
            return log4NetLevel ?? Level.All;
        }

        public bool SetLogMessageTraceLevel(Level level)
        {
            return ServiceLocator.SettingsService.UpdateSetting(ConfigKeyLogMessageTraceLevel, level.ToString());
        }

        public string [] GetAllowedRoles()
        {
            var allowedRoles = ServiceLocator.SettingsService.GetSetting(ConfigKeyAllowedRoles, string.Empty);
            return allowedRoles.Split(',');
        }

        public bool SetAllowedRoles(string[] allowedRoles)
        {
            return ServiceLocator.SettingsService.UpdateSetting(ConfigKeyAllowedRoles, string.Join(",", allowedRoles));
        }

        public bool IsAllowed(UserInfo user)
        {
            if (user.IsSuperUser) return true;
            if (user.UserID == Null.NullInteger) return false;

            var allowedRoles = GetAllowedRoles();
            return allowedRoles.Contains(DotNetNuke.Common.Globals.glbRoleAllUsersName) || allowedRoles.Any(e => user.Roles.Contains(e));
        }
    }
}
