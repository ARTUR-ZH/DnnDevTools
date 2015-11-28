namespace weweave.DnnDevTools.Service.Config
{
    interface IConfigService
    {
        bool SetEnableMailCatch(bool enableMailCatch);

        bool GetEnableMailCatch();
    }
}
