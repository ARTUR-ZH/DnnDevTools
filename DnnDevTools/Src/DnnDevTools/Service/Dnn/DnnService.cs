using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;

namespace weweave.DnnDevTools.Service.Dnn
{
    internal class DnnService : ServiceBase, IDnnService
    {
        public DnnService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        private static string CachePrefix => "DnnDevTools";

        private static string PrefixCacheKey(string cacheKey)
        {
            return $"{CachePrefix}{cacheKey}";
        }

        public T GetCachedObject<T>(int cacheTime, string cacheKey, Func<T> callback)
        {
            var performanceSettings = (int)Host.PerformanceSetting;

            // Do not cache if caching is disabled or caching time is to small
            if (performanceSettings <= 0 || cacheTime < performanceSettings) return callback();

            var prefixedCacheKey = PrefixCacheKey(cacheKey);

            var data = DataCache.GetCachedData<object>(new CacheItemArgs(prefixedCacheKey, cacheTime / performanceSettings), args => callback());
            if (data is T)
            {
                return (T)data;
            }
            ClearCache(prefixedCacheKey);
            return callback();
        }

        public void ClearCache(string cacheKeyOrPrefix)
        {
            DataCache.ClearCache(cacheKeyOrPrefix);
        }
    }
}
