using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Installer.Packages;

namespace weweave.DnnDevTools.Util
{
    internal static class ModuleUtil
    {

        public static bool IsInstalled()
        {
            var package = PackageController.Instance.GetExtensionPackage(Null.NullInteger, e => e.FriendlyName == Globals.ModuleFriendlyName);
            return !Null.IsNull(package);
        }

    }
}
