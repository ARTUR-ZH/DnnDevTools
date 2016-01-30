using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules.Definitions;

namespace weweave.DnnDevTools.Util
{
    internal static class ModuleUtil
    {

        public static bool IsInstalled()
        {
            var moduleDefinition = ModuleDefinitionController.GetModuleDefinitionByFriendlyName(Globals.ModuleFriendlyName);
            return !Null.IsNull(moduleDefinition);
        }

    }
}
