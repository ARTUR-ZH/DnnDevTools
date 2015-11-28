namespace weweave.DnnDevTools.Service.Config
{
    internal class ConfigService : ServiceBase, IConfigService
    {
        public ConfigService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public void SetEnableMailCatch(bool enableMailCatch)
        {
            ServiceLocator.SettingsService.UpdateSetting("EnableMailCatch", enableMailCatch.ToString());
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
