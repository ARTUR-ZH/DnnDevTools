namespace weweave.DnnDevTools.Service.Config
{
    interface IConfigService
    {
        void SetEnableMailCatch(bool enableMailCatch);

        bool GetEnableMailCatch();
    }
}
