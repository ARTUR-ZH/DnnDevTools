using System.Threading;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Web.Api;

namespace weweave.DnnDevTools.Api
{
    internal class SuperUserAuthorizeAttribute : AuthorizeAttributeBase
    {
        public override bool IsAuthorized(AuthFilterContext context)
        {
            var principal = Thread.CurrentPrincipal;
            return principal.Identity.IsAuthenticated &&
                   PortalController.Instance.GetCurrentPortalSettings().UserInfo.IsSuperUser;
        }
    }
}
