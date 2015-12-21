using DotNetNuke.Web.Api;

namespace weweave.DnnDevTools
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("DnnDevTools", "default", "{controller}/{action}", new { action = "Index" }, new[] { "weweave.DnnDevTools.Api.Controller" });
        }
    }
}
