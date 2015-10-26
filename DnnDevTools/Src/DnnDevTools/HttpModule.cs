using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Framework;

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

            var bodyControl = page?.FindControl("Body") as HtmlContainerControl;
            if (bodyControl == null) return;

            var toolbarHtml = @"<script src=""/desktopmodules/DnnDevTools/Scripts/jquery.signalR-2.2.0.js""></script><script src=""signalr/hubs""></script>";
            toolbarHtml += System.IO.File.ReadAllText(
                HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/Toolbar.html")
            );

            var scriptControl = new LiteralControl { Text = toolbarHtml };
            bodyControl.Controls.Add(scriptControl);
        }
    }
}
