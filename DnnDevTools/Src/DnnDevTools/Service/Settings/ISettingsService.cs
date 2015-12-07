namespace weweave.DnnDevTools.Service.Settings
{
    public interface ISettingsService
    {
        string GetSetting(string key, string defaultValue);

        bool UpdateSetting(string key, string value);
    }
}
