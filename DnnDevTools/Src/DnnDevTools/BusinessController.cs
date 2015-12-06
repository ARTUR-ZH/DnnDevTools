using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Definitions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Upgrade;

namespace weweave.DnnDevTools
{
    internal class BusinessController : IUpgradeable
    {
        public string UpgradeModule(string version)
        {
            try
            {
                var moduleDefinition = ModuleDefinitionController.GetModuleDefinitionByFriendlyName("DNN Dev Tools");
                if (moduleDefinition == null) return "Success";

                const string tabName = "DNN Dev Tools";
                const string description = "Manage DNN Dev Tools";
                const string moduleTitle = "DNN Dev Tools";
                const string tabIconFile = "~/Icons/Sigma/Files_16X16_Standard.png";
                const string tabIconFileLarge = "~/Icons/Sigma/Files_32X32_Standard.png";

                // Add host page
                var hostPage = Upgrade.AddHostPage(
                    tabName,
                    description,
                    tabIconFile,
                    tabIconFileLarge,
                    true);

                // Restore host page (if it has been deleted)
                if (hostPage.IsDeleted)
                {
                    hostPage.IsDeleted = false;
                    new TabController().UpdateTab(hostPage);
                }

                // Add module to host page
                Upgrade.AddModuleToPage(
                    hostPage,
                    moduleDefinition.ModuleDefID,
                    moduleTitle,
                    tabIconFile,
                    true
                    );

                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }

        }
    }
}
