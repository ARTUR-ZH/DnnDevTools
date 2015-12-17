using log4net.Core;

namespace weweave.DnnDevTools.Service.Config
{
    public interface IConfigService
    {
        bool SetEnable(bool status);

        bool GetEnable();

        bool SetEnableMailCatch(bool status);

        bool GetEnableMailCatch();

        Level GetLogMessageLevel();

        bool SetLogMessageLevel(Level level);
    }
}
