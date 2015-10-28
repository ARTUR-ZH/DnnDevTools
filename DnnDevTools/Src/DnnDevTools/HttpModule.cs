using System;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Framework;
using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Client.Providers;

namespace weweave.DnnDevTools
{
    public class HttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        public void Dispose()
        {

        }

        private static void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            var page = context?.Handler as CDefault;
            if (page == null) return;

            page.Init += OnPageInit;

        }

        private static void OnPageInit(object sender, EventArgs e)
        {
            var page = (Page)sender;

            ServicesFramework.Instance.RequestAjaxScriptSupport();

            var bodyControl = page?.FindControl("Body") as HtmlContainerControl;
            if (bodyControl == null) return;

            ClientResourceManager.RegisterScript(page, "~/desktopmodules/DnnDevTools/Scripts/jquery.signalR-2.2.0.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);
            var toolbarHtml = $@"<script src=""{HostingEnvironment.ApplicationVirtualPath}/signalr/hubs""></script>";
            toolbarHtml += System.IO.File.ReadAllText(
                HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/Toolbar.html")
            );

            var scriptControl = new LiteralControl { Text = toolbarHtml };
            bodyControl.Controls.Add(scriptControl);
        }
    }
}
