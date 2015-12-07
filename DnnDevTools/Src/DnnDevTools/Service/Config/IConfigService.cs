namespace weweave.DnnDevTools.Service.Config
{
    public interface IConfigService
    {
        bool SetEnable(bool status);

        bool GetEnable();

        bool SetEnableMailCatch(bool status);

        bool GetEnableMailCatch();
    }
}
