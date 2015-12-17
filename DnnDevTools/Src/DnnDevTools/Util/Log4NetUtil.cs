using System;
using System.Linq;
using log4net.Core;

namespace weweave.DnnDevTools.Util
{
    class Log4NetUtil
    {
        public static Level ParseLevel(string level)
        {
            var loggerRepository = LoggerManager.GetAllRepositories().First();

            if (loggerRepository == null)
            {
                throw new Exception("No logging repositories defined");
            }

            var stronglyTypedLevel = loggerRepository.LevelMap[level];
            return stronglyTypedLevel;
        }
    }
}
