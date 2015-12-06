namespace weweave.DnnDevTools.Service.Config
{
    interface IConfigService
    {
        bool SetEnable(bool status);

        bool GetEnable();

        bool SetEnableMailCatch(bool status);

        bool GetEnableMailCatch();
    }
}
