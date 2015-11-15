namespace weweave.DnnDevTools.Service.Settings
{
    internal interface ISettingsService
    {
        string GetSetting(string key, string defaultValue);

        bool UpdateSetting(string key, string value);
    }
}
