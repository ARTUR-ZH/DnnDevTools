using System;

namespace weweave.DnnDevTools.Service.Dnn
{
    public interface IDnnService
    {
        T GetCachedObject<T>(int cacheTime, string cacheKey, Func<T> callback);

        void ClearCache(string cacheKeyOrPrefix);
    }
}
