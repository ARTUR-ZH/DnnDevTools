using System.Security.Principal;
using DotNetNuke.Entities.Users;
using Microsoft.AspNet.SignalR;

namespace weweave.DnnDevTools.SignalR
{
    public class SuperUserAuthorizeAttribute : AuthorizeAttribute
    {

        protected override bool UserAuthorized(IPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated) return false;

            var dnnUser = UserController.GetUserByName(user.Identity.Name);
            return dnnUser.IsSuperUser;
        }
    }
}