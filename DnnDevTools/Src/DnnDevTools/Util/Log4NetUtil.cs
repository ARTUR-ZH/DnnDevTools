using System;
using log4net.Core;

namespace weweave.DnnDevTools.Util
{
    internal static class Log4NetUtil
    {
        public static Level ParseLevel(string level)
        {
            if ("info".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Info;
            if ("alert".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Alert;
            if ("all".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.All;
            if ("critical".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Critical;
            if ("debug".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Debug;
            if ("emergency".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Emergency;
            if ("error".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Error;
            if ("fatal".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Fatal;
            if ("fine".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Fine;
            if ("finer".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Finer;
            if ("finest".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Finest;
            if ("notice".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Notice;
            if ("all".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.All;
            if ("off".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Off;
            if ("severe".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Severe;
            if ("trace".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Trace;
            if ("verbose".Equals(level, StringComparison.OrdinalIgnoreCase))
                return Level.Verbose;
            return "warn".Equals(level, StringComparison.OrdinalIgnoreCase) ? Level.Warn : null;
        }
    }
}
