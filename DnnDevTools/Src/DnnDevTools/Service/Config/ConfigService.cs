namespace weweave.DnnDevTools.Service.Config
{
    internal class ConfigService : ServiceBase, IConfigService
    {
        public ConfigService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public void SetEnableMailCatch(bool enableMailCatch)
        {
            ServiceLocatorFactory.Instance.SettingsService.UpdateSetting("EnableMailCatch", enableMailCatch.ToString());
        }

        public bool GetEnableMailCatch()
        {
            bool enableMailCatch;
            return bool.TryParse(
                ServiceLocatorFactory.Instance.SettingsService.GetSetting("EnableMailCatch", false.ToString()),
                out enableMailCatch) && enableMailCatch;
        }

    }
}
