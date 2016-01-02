using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;

namespace weweave.DnnDevTools.Service.Settings
{
    internal class SettingsService : ServiceBase, ISettingsService
    {
        private const string CacheKey = "Settings";

        internal SettingsService(IServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        public string GetSetting(string key, string defaultValue)
        {
            var settings = ServiceLocator.DnnService.GetCachedObject(60, CacheKey, LoadSettings);
            string value;
            return settings.TryGetValue(key, out value) ? value : defaultValue;
        }

        public bool UpdateSetting(string key, string value)
        {
            var oldValue = GetSetting(key, null);
            if (oldValue == value) return false;
            SaveSetting(key, value);
            ServiceLocator.DnnService.ClearCache(CacheKey);
            return true;
        }

        #region DB Access

        private static Dictionary<string, string> LoadSettings()
        {
            var dictionary = new Dictionary<string, string>();
            var settings = DataProvider.Instance().ExecuteReader("DnnDevTools_GetSettings", new object[0]);
            try
            {
                while (settings.Read())
                {
                    if (settings.IsDBNull(0) && settings.IsDBNull(1)) continue;
                    dictionary.Add(settings.GetString(0), settings.GetString(1));
                }
            }
            finally
            {
                CBO.CloseDataReader(settings, true);
            }

            return dictionary;
        }


        private static void SaveSetting(string key, string value)
        {
            DataProvider.Instance().ExecuteNonQuery("DnnDevTools_UpdateSettings", (object)key, (object)value);
        }

        #endregion
    }
}
