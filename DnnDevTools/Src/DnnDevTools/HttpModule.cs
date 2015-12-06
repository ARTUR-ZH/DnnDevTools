using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Framework;
using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Client.Providers;
using Newtonsoft.Json;

namespace weweave.DnnDevTools
{
    public class HttpModule : IHttpModule
    {
        private class LocalizeResourceFilter : MemoryStream
        {
            private readonly Stream _outputStream;
            public LocalizeResourceFilter(Stream outputStream)
            {
                _outputStream = outputStream;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                var contentInBuffer = Encoding.UTF8.GetString(buffer);
                var rsxr = new ResXResourceReader(HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/App_LocalResources/Overlay.resx"));
                contentInBuffer = rsxr.Cast<DictionaryEntry>().Aggregate(contentInBuffer, (current, d) => current.Replace($"[res:{d.Key}]", d.Value.ToString()));
                _outputStream.Write(Encoding.UTF8.GetBytes(contentInBuffer), offset, Encoding.UTF8.GetByteCount(contentInBuffer));
            }
        }


        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
            context.ReleaseRequestState += OnReleaseRequestState;
        }

        private static void OnReleaseRequestState(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;

            if (!"~/DesktopModules/DnnDevTools/Overlay.aspx".Equals(request.AppRelativeCurrentExecutionFilePath, StringComparison.OrdinalIgnoreCase))
                return;

            HttpContext.Current.Response.Filter = new LocalizeResourceFilter(HttpContext.Current.Response.Filter);
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
            // Skip if user is no super user
            if (!PortalController.Instance.GetCurrentPortalSettings().UserInfo.IsSuperUser)
                return;

            // Skip for DNN popups
            if (UrlUtils.InPopUp()) return;

            var page = (Page)sender;

            ServicesFramework.Instance.RequestAjaxScriptSupport();

            var bodyControl = page?.FindControl("Body") as HtmlContainerControl;
            if (bodyControl == null) return;

            // Register SignalR
            ClientResourceManager.RegisterScript(page, "~/desktopmodules/DnnDevTools/Scripts/jquery.signalR-2.2.0.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);

            // Get (static) toolbar html
            var toolbarHtml = File.ReadAllText(
                HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/Toolbar.html")
            );

            // Replace toolbar resources
            var rsxr = new ResXResourceReader(HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/App_LocalResources/Toolbar.resx"));
            toolbarHtml = rsxr.Cast<DictionaryEntry>().Aggregate(toolbarHtml, (current, d) => current.Replace($"[res:{d.Key}]", d.Value.ToString()));

            // Build JavaScript config
            var javaScriptConfig = new Dictionary<string, object>
            {
                ["enableMailCatch"] = ServiceLocatorFactory.Instance.ConfigService.GetEnableMailCatch(),
                ["baseUrl"] = $"{HostingEnvironment.ApplicationVirtualPath}/DesktopModules/DnnDevTools/"
            };

            // Inject HTML into end of body
            var html = $@"<script src=""{HostingEnvironment.ApplicationVirtualPath}/signalr/hubs""></script>";
            html += $@"<script type=""text/javascript"">window.dnnDevTools={JsonConvert.SerializeObject(javaScriptConfig)}</script>";
            html += toolbarHtml;
            var scriptControl = new LiteralControl { Text = html };
            bodyControl.Controls.Add(scriptControl);
        }

    }

}
