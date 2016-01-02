using System.Threading;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Web.Api;

namespace weweave.DnnDevTools.Api
{
    internal class IsAllowedAuthorizeAttribute : AuthorizeAttributeBase
    {
        public override bool IsAuthorized(AuthFilterContext context)
        {
            var principal = Thread.CurrentPrincipal;
            return principal.Identity.IsAuthenticated &&
                ServiceLocatorFactory.Instance.ConfigService.IsAllowed(PortalController.Instance.GetCurrentPortalSettings().UserInfo);
        }
    }
}
